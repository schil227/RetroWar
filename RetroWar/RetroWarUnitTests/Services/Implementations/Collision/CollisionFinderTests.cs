using NUnit.Framework;
using RetroWar.Models.Collisions;
using RetroWar.Models.Sprites;
using RetroWar.Models.Sprites.HitBoxes;
using RetroWar.Services.Implementations.Collision;
using RetroWar.Services.Interfaces.Collision;
using System.Linq;

namespace RetroWarUnitTests.Services.Implementations.Collision
{
    public class CollisionFinderTests
    {
        private ICollisionFinder sut;

        [SetUp]
        public void SetUp()
        {
            sut = new CollisionFinder();
        }

        [Test]
        public void FindCollisions_WhenHitBoxesNotInCollision_ReturnsEmptySet()
        {
            // Arrange
            var normal = new Sprite
            {
                X = 0,
                Y = 0
            };

            var based = new Sprite
            {
                X = 100,
                Y = 100
            };

            var normalHitBox = new HitBox(0, 0, 10, 10);
            var basedHitBox = new HitBox(0, 0, 10, 10);

            // Act
            var result = sut.FindCollisions(normal, based, normalHitBox, basedHitBox);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Findcollisions_WhenBottomRightCornerInBaseHitBox_ReturnsBottomRightCollision()
        {
            // Arrange
            var normal = new Sprite
            {
                X = 0,
                Y = 0
            };

            var based = new Sprite
            {
                X = 4,
                Y = 5
            };

            var normalHitBox = new HitBox(0, 0, 10, 10);
            var basedHitBox = new HitBox(0, 0, 10, 10);

            // Act
            var result = sut.FindCollisions(normal, based, normalHitBox, basedHitBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));

            var collisionResolution = result.First();

            Assert.That(collisionResolution.CollisionPoint, Is.EqualTo(PointInCollision.BottomRight));
            Assert.That(collisionResolution.DeltaX, Is.EqualTo(-6));
            Assert.That(collisionResolution.DeltaY, Is.EqualTo(-5));
        }

        [Test]
        public void Findcollisions_WhenBottomLeftCornerInBaseHitBox_ReturnsBottomLeftCollision()
        {
            // Arrange
            var normal = new Sprite
            {
                X = 6,
                Y = 0
            };

            var based = new Sprite
            {
                X = 0,
                Y = 5
            };

            var normalHitBox = new HitBox(0, 0, 10, 10);
            var basedHitBox = new HitBox(0, 0, 10, 10);

            // Act
            var result = sut.FindCollisions(normal, based, normalHitBox, basedHitBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));

            var collisionResolution = result.First();

            Assert.That(collisionResolution.CollisionPoint, Is.EqualTo(PointInCollision.BottomLeft));
            Assert.That(collisionResolution.DeltaX, Is.EqualTo(4));
            Assert.That(collisionResolution.DeltaY, Is.EqualTo(-5));
        }

        [Test]
        public void Findcollisions_WhenTopLeftCornerInBaseHitBox_ReturnsTopLeftCollision()
        {
            // Arrange
            var normal = new Sprite
            {
                X = 6,
                Y = 5
            };

            var based = new Sprite
            {
                X = 0,
                Y = 0
            };

            var normalHitBox = new HitBox(0, 0, 10, 10);
            var basedHitBox = new HitBox(0, 0, 10, 10);

            // Act
            var result = sut.FindCollisions(normal, based, normalHitBox, basedHitBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));

            var collisionResolution = result.First();

            Assert.That(collisionResolution.CollisionPoint, Is.EqualTo(PointInCollision.TopLeft));
            Assert.That(collisionResolution.DeltaX, Is.EqualTo(4));
            Assert.That(collisionResolution.DeltaY, Is.EqualTo(5));
        }

        [Test]
        public void Findcollisions_WhenTopRightCornerInBaseHitBox_ReturnsTopRightCollision()
        {
            // Arrange
            var normal = new Sprite
            {
                X = 0,
                Y = 6
            };

            var based = new Sprite
            {
                X = 5,
                Y = 0
            };

            var normalHitBox = new HitBox(0, 0, 10, 10);
            var basedHitBox = new HitBox(0, 0, 10, 10);

            // Act
            var result = sut.FindCollisions(normal, based, normalHitBox, basedHitBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));

            var collisionResolution = result.First();

            Assert.That(collisionResolution.CollisionPoint, Is.EqualTo(PointInCollision.TopRight));
            Assert.That(collisionResolution.DeltaX, Is.EqualTo(-5));
            Assert.That(collisionResolution.DeltaY, Is.EqualTo(4));
        }

        [Test]
        public void Findcollisions_WhenTopRightAndLeftCornerInBaseHitBox_ReturnsBothCollisions()
        {
            // Arrange
            var normal = new Sprite
            {
                X = 1,
                Y = 6
            };

            var based = new Sprite
            {
                X = 0,
                Y = 0
            };

            var normalHitBox = new HitBox(0, 0, 2, 10);
            var basedHitBox = new HitBox(0, 0, 10, 10);

            // Act
            var result = sut.FindCollisions(normal, based, normalHitBox, basedHitBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));

            var topRightCollision = result.First(r => r.CollisionPoint == PointInCollision.TopRight);
            var topLeftCollision = result.First(r => r.CollisionPoint == PointInCollision.TopLeft);

            Assert.That(topRightCollision.DeltaX, Is.EqualTo(-3));
            Assert.That(topRightCollision.DeltaY, Is.EqualTo(4));

            Assert.That(topLeftCollision.DeltaX, Is.EqualTo(9));
            Assert.That(topLeftCollision.DeltaY, Is.EqualTo(4));
        }

        [Test]
        public void Findcollisions_WhenNormalContainedInBaseHitBox_ReturnsAllCollisions()
        {
            // Arrange
            var normal = new Sprite
            {
                X = 1,
                Y = 2
            };

            var based = new Sprite
            {
                X = 0,
                Y = 0
            };

            var normalHitBox = new HitBox(0, 0, 2, 2);
            var basedHitBox = new HitBox(0, 0, 10, 10);

            // Act
            var result = sut.FindCollisions(normal, based, normalHitBox, basedHitBox);

            // Assert
            Assert.That(result.Count, Is.EqualTo(4));

            var topRightCollision = result.First(r => r.CollisionPoint == PointInCollision.TopRight);
            var topLeftCollision = result.First(r => r.CollisionPoint == PointInCollision.TopLeft);
            var bottomRightCollision = result.First(r => r.CollisionPoint == PointInCollision.BottomRight);
            var bottomLeftCollision = result.First(r => r.CollisionPoint == PointInCollision.BottomLeft);

            Assert.That(topRightCollision.DeltaX, Is.EqualTo(-3));
            Assert.That(topRightCollision.DeltaY, Is.EqualTo(8));

            Assert.That(topLeftCollision.DeltaX, Is.EqualTo(9));
            Assert.That(topLeftCollision.DeltaY, Is.EqualTo(8));

            Assert.That(bottomLeftCollision.DeltaX, Is.EqualTo(9));
            Assert.That(bottomLeftCollision.DeltaY, Is.EqualTo(-4));

            Assert.That(bottomRightCollision.DeltaX, Is.EqualTo(-3));
            Assert.That(bottomRightCollision.DeltaY, Is.EqualTo(-4));
        }
    }
}
