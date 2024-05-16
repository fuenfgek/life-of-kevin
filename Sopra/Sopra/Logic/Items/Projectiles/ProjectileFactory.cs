using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sopra.ECS;
using Sopra.Logic.Collision;
using Sopra.Logic.Render;

namespace Sopra.Logic.Items.Projectiles
{
    public sealed class ProjectileFactory
    {
        private readonly Engine mEngine;

        private readonly Dictionary<ProjectileTypes, SimpleSpriteC> mTextureDict;
        private readonly Dictionary<ProjectileTypes, HitboxC> mHitboxDict;
        private readonly Dictionary<string, ComplexSprite> mAnimationDict;


        internal ProjectileFactory(Engine engine, ContentManager content)
        {
            mEngine = engine;

            mTextureDict = new Dictionary<ProjectileTypes, SimpleSpriteC>()
            {
                {ProjectileTypes.PistolBullet,
                    new SimpleSpriteC(0, content.Load<Texture2D>("test_sprites/shot1"), new Vector2(2))},
                {ProjectileTypes.RifleBullet,
                    new SimpleSpriteC(0, content.Load<Texture2D>("test_sprites/shot1"), new Vector2(2))},
                {ProjectileTypes.MinigunBullet,
                    new SimpleSpriteC(0, content.Load<Texture2D>("test_sprites/shot1"), new Vector2(2))}
            };

            mHitboxDict = new Dictionary<ProjectileTypes, HitboxC>()
            {
                {ProjectileTypes.PistolBullet, new HitboxC(20)},
                {ProjectileTypes.RifleBullet, new HitboxC(20)},
                {ProjectileTypes.MinigunBullet, new HitboxC(20)},
                {ProjectileTypes.RocketBullet, new HitboxC(20)}
            };

            mAnimationDict = new Dictionary<string, ComplexSprite>()
            {
                {"default_default",
                    new ComplexSprite(content.Load<Texture2D>("Test_sprites/shot1"), new Vector2(2))},
                { "default_explosion",
                    new ComplexSprite(content.Load<Texture2D>("items/projectiles/rocket_explosion_set"), new Vector2(64), 9, 0.5f)}
            };


        }

        /// <summary>
        /// Spawn a bullet with the given parameters.
        /// </summary>
        /// <param name="gunOwnerId">ID that should be ignored during collision testing.</param>
        /// <param name="type"></param>
        /// <param name="pos"></param>
        /// <param name="facing"></param>
        /// <param name="time"></param>
        internal void SpawnProjectile(
            int? gunOwnerId,
            ProjectileTypes type,
            Vector2 pos,
            Vector2 facing,
            GameTime time)
        {
            var bullet = mEngine.EntityManager.Create()
                .AddComponent(new TransformC(pos, facing));

            switch (type)
            {
                case ProjectileTypes.PistolBullet:
                    bullet.AddComponent(mTextureDict[type])
                        .AddComponent(mHitboxDict[type])
                        .AddComponent(new SimpleBulletC(3, 1, 3000, gunOwnerId));
                    break;

                case ProjectileTypes.RifleBullet:
                    bullet.AddComponent(mTextureDict[type])
                        .AddComponent(mHitboxDict[type])
                        .AddComponent(new SimpleBulletC(5, 2, 3000, gunOwnerId));
                    break;

                case ProjectileTypes.MinigunBullet:
                    bullet.AddComponent(mTextureDict[type])
                        .AddComponent(mHitboxDict[type])
                        .AddComponent(new SimpleBulletC(10, 1, 3000, gunOwnerId));
                    break;

                case ProjectileTypes.RocketBullet:
                    bullet.AddComponent(new AnimationC(0, mAnimationDict))
                        .AddComponent(mHitboxDict[type])
                        .AddComponent(new RocketBulletC(4, 5, 3000, gunOwnerId))
                        ;
                    break;


                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
