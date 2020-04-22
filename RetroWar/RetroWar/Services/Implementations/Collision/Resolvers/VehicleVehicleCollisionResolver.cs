﻿using RetroWar.Models.Collisions;
using RetroWar.Models.Collisions.Resolvers;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.Vehicles;
using RetroWar.Services.Interfaces.Collision;
using RetroWar.Services.Interfaces.Collision.Resolvers;
using RetroWar.Services.Interfaces.Collision.Resolvers.Helpers;
using RetroWar.Services.Interfaces.Helpers.Collision;
using RetroWar.Services.Interfaces.Helpers.Model;
using System;

namespace RetroWar.Services.Implementations.Collision.Resolvers
{
    public class VehicleVehicleCollisionResolver : ICollisionResolver
    {
        private readonly IResolverHelper resolverHelper;
        private readonly IFaceHelper faceHelper;
        private readonly ICollisionFinder collisionFinder;
        private readonly ICarrierRiderResolver carrierRiderResolver;
        public VehicleVehicleCollisionResolver
            (
                IResolverHelper resolverHelper,
                IFaceHelper faceHelper,
                ICollisionFinder collisionFinder,
                ICarrierRiderResolver carrierRiderResolver
            )
        {
            this.resolverHelper = resolverHelper;
            this.faceHelper = faceHelper;
            this.collisionFinder = collisionFinder;
            this.carrierRiderResolver = carrierRiderResolver;
        }

        public bool ResolveCollision(Sprite normal, Sprite based, CollisionResolution collisionResolution)
        {
            Vehicle normalVehicle = null;
            Vehicle basedVehicle = null;

            if (normal is Vehicle && based is Vehicle)
            {
                normalVehicle = (Vehicle)normal;
                basedVehicle = (Vehicle)based;
            }
            else
            {
                return false;
            }

            if (collisionFinder.IsOnTopOf(normal, based))
            {
                HandleVerticleCollisionResolution(normal, based);
                return true;
            }
            else if (collisionFinder.IsOnTopOf(based, normal))
            {
                HandleVerticleCollisionResolution(based, normal);
                return true;
            }

            // if based is moving faster than normal, get collision resolution WRT based sprite instead.
            if (BasedFasterThanNormal(normal, based))
            {
                // get collision face relative to based, then pick the opposite face cause everything else is WRT normal
                collisionResolution = collisionFinder.FindCollisionResolutionFace(based, normal, collisionResolution.DeltaTime);

                if (collisionResolution.PrimaryFace == null)
                {
                    return true;
                }

                collisionResolution.PrimaryFace = faceHelper.GetOppositeFace(collisionResolution.PrimaryFace.Value);
            }

            if (collisionResolution.PrimaryFace == null)
            {
                return true;
            }

            var difference = faceHelper.GetFaceDifference(normal, faceHelper.GetOppositeFace(collisionResolution.PrimaryFace.Value), based);

            float normalVelocity = 0;
            float basedVelocity = 0;

            if (collisionResolution.PrimaryFace == Face.Left || collisionResolution.PrimaryFace == Face.Right)
            {
                normalVelocity = normal.X - normal.OldX;
                basedVelocity = based.X - based.OldX;
            }
            else
            {
                normalVelocity = normal.Y - normal.OldY;
                basedVelocity = based.X - based.OldX;
            }

            var normalResolution = new ResolutionVector
            {
                Magnitude = difference / 2,
                Direction = resolverHelper.DirectionToResolveIn(collisionResolution.PrimaryFace.Value, true)
            };

            var basedResolution = new ResolutionVector
            {
                Magnitude = difference / 2,
                Direction = resolverHelper.DirectionToResolveIn(collisionResolution.PrimaryFace.Value, false)
            };

            if (normalVelocity == 0 && basedVelocity == 0)
            {
                // resolve them as is
            }
            else if (VelocitesAgainstEachother(normalVelocity, basedVelocity))
            {
                normalResolution.Magnitude = AdjustDifferenceByRelatedVelocities(difference, normalVelocity, basedVelocity);
                basedResolution.Magnitude = difference - normalResolution.Magnitude;
            }
            else
            {
                // going the same direction
                if (normalVelocity >= basedVelocity)
                {
                    normalResolution.Magnitude = 0;
                }
                else
                {
                    normalResolution.Magnitude = difference;
                }

                basedResolution.Magnitude = difference - normalResolution.Magnitude;
            }

            ResolveVehicles(normal, based, normalResolution, basedResolution);

            return true;
        }

        private void HandleVerticleCollisionResolution(Sprite rider, Sprite carrier)
        {
            var riderVerticalResolution = carrierRiderResolver.GetRiderVerticleResolutionVector(rider, carrier);

            ResolveVehicles(rider, carrier, riderVerticalResolution, new ResolutionVector { Direction = Direction.Down, Magnitude = 0 });

            var riderHorizontalResolution = carrierRiderResolver.GetRiderHorizontalResolutionVector(rider, carrier);

            ResolveVehicle(rider, riderHorizontalResolution);
        }

        private bool BasedFasterThanNormal(Sprite normal, Sprite based)
        {
            var normalXSpeed = Math.Abs(normal.X - normal.OldX);
            var normalYSpeed = Math.Abs(normal.Y - normal.OldY);

            var basedXSpeed = Math.Abs(based.X - based.OldX);
            var basedYSpeed = Math.Abs(based.Y - based.OldY);

            return basedXSpeed > normalXSpeed && basedXSpeed > normalYSpeed
                || basedYSpeed > normalXSpeed && basedYSpeed > normalYSpeed;
        }

        private void ResolveVehicles(Sprite normal, Sprite based, ResolutionVector normalResolution, ResolutionVector basedResolution)
        {
            normalResolution.Magnitude += ResolveVehicle(based, basedResolution);
            ResolveVehicle(normal, normalResolution);
        }

        private float ResolveVehicle(Sprite sprite, ResolutionVector resolution)
        {
            var remainder = 0.0f;

            if (resolution.Magnitude == 0)
            {
                return remainder;
            }

            // check for limits (e.g. tiles in the way) here

            switch (resolution.Direction)
            {
                case Direction.Right:
                    {
                        sprite.X += resolution.Magnitude;
                        break;
                    }
                case Direction.Left:
                    {
                        sprite.X -= resolution.Magnitude;
                        break;
                    }
                case Direction.Up:
                    {
                        sprite.Y -= resolution.Magnitude;
                        break;
                    }
                case Direction.Down:
                    {
                        sprite.Y += resolution.Magnitude;
                        break;
                    }
            }

            return Math.Abs(remainder);
        }

        private bool VelocitesAgainstEachother(float normalVelocity, float basedVelocity)
        {
            return normalVelocity >= 0 && basedVelocity <= 0
                || normalVelocity <= 0 && basedVelocity >= 0;
        }

        // Names are hard.
        //
        // basically what's going on here is since normal and based are pushing against eachother, we're determining 
        // how much one should move back vs the other. To do that we plug it into the following formula:
        //
        // ▲z = the difference to move to get out of collision (effectively ▲x or ▲y)
        // VzN = the velocity of normal N on the given axis 
        // VzB = the velocity of based B on the given axis
        //
        // adjDist = ▲z - (▲z½ + ▲z(λ))
        //      where λ = (VzN - VzB) / (VzN + VzB)
        //
        // For example, when ▲z = 10, VzN = 50 and VzB = 25:
        //      λ = 25/75 => ⅓,
        //  
        //      adjDist = 10 - (5 + 5(⅓)) => 10 - 6.666 => 3.333
        //
        // Meaning, the normal will move "back" 3.333 instead of 5, the based will pick up the remaining difference (6.666)
        //
        // Note that this method decays relatively slowly; making it have a sharper pushback (e.g. λ = ½ instead of ⅓) would
        // probably be more effective
        private float AdjustDifferenceByRelatedVelocities(float deltaZ, float velocityZN, float velocityZB)
        {
            var absVN = Math.Abs(velocityZN);
            var absVB = Math.Abs(velocityZB);

            return deltaZ - (deltaZ * (1.0f / 2.0f) + (deltaZ * (1.0f / 2.0f) * ((absVN - absVB) / (absVN + absVB))));
        }
    }
}