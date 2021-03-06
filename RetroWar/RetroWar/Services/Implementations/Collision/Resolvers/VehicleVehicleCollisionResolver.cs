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

            if (collisionFinder.IsOnTopOf(normalVehicle, basedVehicle))
            {
                HandleVerticleCollisionResolution(normalVehicle, basedVehicle);
                return true;
            }
            else if (collisionFinder.IsOnTopOf(basedVehicle, normalVehicle))
            {
                HandleVerticleCollisionResolution(basedVehicle, normalVehicle);
                return true;
            }

            if (collisionFinder.IsRightOf(normalVehicle, basedVehicle))
            {
                collisionResolution.PrimaryFace = Face.Right;
            }
            else
            {
                if (!normalVehicle.StickyCollisionData.ContainsKey(basedVehicle.SpriteId))
                {
                    normalVehicle.StickyCollisionData.Add(based.SpriteId, Face.Left);
                }

                if (!basedVehicle.StickyCollisionData.ContainsKey(normalVehicle.SpriteId))
                {
                    basedVehicle.StickyCollisionData.Add(normal.SpriteId, Face.Right);
                }

                collisionResolution.PrimaryFace = Face.Left;
            }

            var difference = faceHelper.GetFaceDifference(normalVehicle, faceHelper.GetOppositeFace(collisionResolution.PrimaryFace.Value), basedVehicle);

            float normalVelocity = 0;
            float basedVelocity = 0;

            if (collisionResolution.PrimaryFace == Face.Left || collisionResolution.PrimaryFace == Face.Right)
            {
                normalVelocity = normalVehicle.X - normalVehicle.OldX;
                basedVelocity = basedVehicle.X - basedVehicle.OldX;
            }
            else
            {
                normalVelocity = normalVehicle.Y - normalVehicle.OldY;
                basedVelocity = basedVehicle.X - basedVehicle.OldX;
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
                ResolveVehicles(normalVehicle, basedVehicle, normalResolution, basedResolution);
                return true;
            }
            else if (VelocitesAgainstEachOther(normalVelocity, basedVelocity))
            {
                normalResolution.Magnitude = AdjustDifferenceByRelatedVelocities(difference, normalVelocity, basedVelocity);
                basedResolution.Magnitude = difference - normalResolution.Magnitude;

                // based is moving more, resolve them first
                if (normalResolution.Magnitude < basedResolution.Magnitude)
                {
                    ResolveVehicles(basedVehicle, normalVehicle, basedResolution, normalResolution);
                }
                else
                {
                    ResolveVehicles(normalVehicle, basedVehicle, normalResolution, basedResolution);
                }

                return true;
            }
            else
            {
                // going the same direction
                if (Math.Abs(normalVelocity) >= Math.Abs(basedVelocity))
                {
                    normalResolution.Magnitude = 0;
                }
                else
                {
                    normalResolution.Magnitude = difference;
                }

                basedResolution.Magnitude = difference - normalResolution.Magnitude;

                if (normalResolution.Magnitude < basedResolution.Magnitude)
                {
                    ResolveVehicles(basedVehicle, normalVehicle, basedResolution, normalResolution);
                }
                else
                {
                    ResolveVehicles(normalVehicle, basedVehicle, normalResolution, basedResolution);
                }

                return true;
            }
        }

        private void HandleVerticleCollisionResolution(Vehicle rider, Vehicle carrier)
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

        private void ResolveVehicles(Vehicle firstToResolve, Vehicle secondToResolve, ResolutionVector firstResolution, ResolutionVector secondResolution)
        {
            secondResolution.Magnitude += ResolveVehicle(firstToResolve, firstResolution);
            ResolveVehicle(secondToResolve, secondResolution);
        }

        private float ResolveVehicle(Vehicle vehicle, ResolutionVector resolution)
        {
            var remainder = 0.0f;

            if (resolution.Magnitude == 0)
            {
                return remainder;
            }

            // check for limits (e.g. tiles in the way) here
            var distanceToTile = carrierRiderResolver.DistanceFromFaceToClosestTile(resolution.Direction, resolution.Magnitude, vehicle);

            remainder = resolution.Magnitude - distanceToTile;

            resolution.Magnitude = distanceToTile;

            switch (resolution.Direction)
            {
                case Direction.Right:
                    {
                        vehicle.OldX = vehicle.X;
                        vehicle.X += resolution.Magnitude;
                        break;
                    }
                case Direction.Left:
                    {
                        vehicle.OldX = vehicle.X;
                        vehicle.X -= resolution.Magnitude;
                        break;
                    }
                case Direction.Up:
                    {
                        vehicle.OldY = vehicle.Y;
                        vehicle.Y -= resolution.Magnitude;
                        vehicle.FallSum = 0;
                        vehicle.IsJumping = false;

                        break;
                    }
                case Direction.Down:
                    {
                        vehicle.OldY = vehicle.Y;
                        vehicle.Y += resolution.Magnitude;
                        vehicle.FallSum = 0;
                        break;
                    }
            }

            return Math.Abs(remainder);
        }

        private bool VelocitesAgainstEachOther(float normalVelocity, float basedVelocity)
        {
            if (normalVelocity == 0 || basedVelocity == 0)
            {
                return false;
            }

            return normalVelocity > 0 && basedVelocity < 0
                || normalVelocity < 0 && basedVelocity > 0;
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
