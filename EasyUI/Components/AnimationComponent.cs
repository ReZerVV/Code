using EasyUI.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;

namespace EasyUI.Components
{
    public class AnimationComponent : IComponent
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = new Vector2(0, 1);
        
        public Color Foreground { get; set; } = Color.White;
        public Color Background { get; set; } = Color.Black;
       
        private int frameIndex = 0;
        public List<string> Frames { get; set; } = new();
        
        private int delayIndex = 0;
        public int Delay { get; set; } = 10;

        public PixelStyle AnimationStyle { get; set; } = PixelStyle.StyleNone;

        public void Input(ConsoleKeyInfo keyInfo)
        {
            
        }

        public void Update()
        {
            delayIndex++;
            if (delayIndex % Delay == 0)
            {
                if (frameIndex + 1 < Frames.Count)
                {
                    frameIndex++;
                }
                else
                {
                    frameIndex = 0;
                }
            }
        }

        public void Layout()
        {
            if (Frames.Count != 0) 
            {
                Size = new Vector2(Frames[frameIndex].Length, 1);
            }
        }

        public void Render(Canvas canvas)
        {
            if (Frames.Count != 0)
            {
                canvas.DrawText(
                    text: Frames[frameIndex],
                    x: Position.x,
                    y: Position.y,
                    foreground: Foreground,
                    background: Background,
                    style: AnimationStyle);
            }
        }
    }
}
