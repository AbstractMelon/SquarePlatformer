using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace SquarePlatformer
{
    public class Player : PhysicsObject
    {
        private bool jumpable = false;
        private bool jumping = false;
        private int jumpHeight = 15;
        private int timeSinceOnFloor = 0;
        private InputProfile inputs = new InputProfile([Keys.Up, Keys.W, Keys.Space], [Keys.Right, Keys.D], [Keys.Down, Keys.S], [Keys.Left, Keys.A]);
        public Player(Vec2 pos) : base("Player", pos, new Vec2(50, 50))
        {
            LevelManager.alivePlayers++;
            affectedByGravity = true;
            pushable = true;
            tags.Add("Player");
            Camera.targets.Add(this);
        }
        public override void Update()
        {
            if (position.y <= -800)
            {
                Kill();
            }
            if (velocity.x > 0) velocity.x --;
            else if (velocity.x < 0) velocity.x ++;
            UpdateControls();
            timeSinceOnFloor++;
        }
        public void UpdateControls()
        {
            if (InputManager.GetKeyDown(inputs.right)) 
            {
                velocity.x += 2;
                if (flipTexture)
                {
                    flipTexture = false;
                }
            }
            if (InputManager.GetKeyDown(inputs.left)) 
            {
                velocity.x -= 2;
                if (!flipTexture)
                {
                    flipTexture = true;
                }
            }
            if (InputManager.GetKeyDown(inputs.up))
            {
                if (jumpable && timeSinceOnFloor <= 10) 
                {
                    Jump(jumpHeight);
                }
            }
            else if (jumping)
            {
                if (velocity.y > 0) velocity.y -= velocity.y * 0.25 * gravity * weight;
            }
            if (InputManager.GetKeyDown(inputs.down))
            {
                velocity.y -= 1;
            }
        }
        public override void CollisionEnd(CollisionInformation info)
        {
            if (info.side == Side.Left || info.side == Side.Right || info.obj.position.y > position.y) return;
            jumpable = true;
            timeSinceOnFloor = 0;
            jumping = false;
        }
        public void Kill()
        {
            ObjectManager.AddToDestroy(this);
            LevelManager.alivePlayers--;
            Camera.targets.Remove(this);
        }
        public void Jump(int height)
        {
            jumpable = false;
            velocity.y = height;
            jumping = true;
        }
    }
}
//TODO: Add side enum and more advanced collision information, and make enemy do all of it's stuff for collision instead of getting help by the player