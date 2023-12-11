using EasyUI.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;

namespace Word.Views
{
    public class WelcomeView : IComponent
    {
        public Color Foreground { get; set; } = AppState.Theme.Foreground;
        public Color Background { get; set; } = AppState.Theme.Background;
        public Color BackgroundDark { get; set; } = AppState.Theme.BackgroundDark;

        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = Vector2.Zero;
        public PixelStyle TextStyle { get; set; } = new PixelStyle { Dim = true, Italic = true };

        public void Input(ConsoleKeyInfo keyInfo)
        {

        }

        public void Update()
        {
            if (AppState.ThemeChanged)
            {
                Foreground = AppState.Theme.Foreground;
                Background = AppState.Theme.Background;
                BackgroundDark = AppState.Theme.BackgroundDark;
            }
        }

        public void Layout()
        {

        }

        public void Render(Canvas canvas)
        {
            RenderLogo(canvas);
            RenderHotkeys(canvas);
        }

        private void RenderLogo(Canvas canvas)
        {
            canvas.DrawText(
                text: "     ██████████     ",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 12,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);
            
            canvas.DrawText(
                text: "  ████████████████  ",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 11,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: " ██████████████████ ",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 10,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: "██████████  ████████",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 9,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: " ███████ ███  ███████",
                x: Position.x + Size.x / 2 - 23 / 2,
                y: Position.y + Size.y / 2 - 8,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: "██████ █████   █████",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 7,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: "███████ ███  ███████",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 6,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: "██████████  ████████",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 5,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: " ██████████████████ ",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 4,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: "  ████████████████  ",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 3,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);

            canvas.DrawText(
                text: "     ██████████     ",
                x: Position.x + Size.x / 2 - 20 / 2,
                y: Position.y + Size.y / 2 - 2,
                foreground: BackgroundDark,
                background: Background,
                style: PixelStyle.StyleNone);
        }

        private void RenderHotkeys(Canvas canvas)
        {
            canvas.DrawText(
                text: "                  ╭──────╮   ╭─────╮   ╭───╮",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 2,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            canvas.DrawText(
                text: "Show All Commands │ Ctrl │ + │ Alt │ + │ P │",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 3,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            canvas.DrawText(
                text: "                  ╰──────╯   ╰─────╯   ╰───╯",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 4,
                foreground: Foreground,
                background: Background,
                style: TextStyle);

            canvas.DrawText(
                text: "            ╭──────╮   ╭───╮",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 5,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            canvas.DrawText(
                text: "Create File │ Ctrl │ + │ N │",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 6,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            canvas.DrawText(
                text: "            ╰──────╯   ╰───╯",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 7,
                foreground: Foreground,
                background: Background,
                style: TextStyle);

            canvas.DrawText(
                text: "           ╭──────╮   ╭───╮",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 8,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            canvas.DrawText(
                text: "Close File │ Ctrl │ + │ Q │",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 9,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            canvas.DrawText(
                text: "           ╰──────╯   ╰───╯",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 10,
                foreground: Foreground,
                background: Background,
                style: TextStyle);

            canvas.DrawText(
                text: "          ╭──────╮   ╭───╮",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 11,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            canvas.DrawText(
                text: "Open File │ Ctrl │ + │ O │",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 12,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            canvas.DrawText(
                text: "          ╰──────╯   ╰───╯",
                x: Position.x + Size.x / 2 - 44 / 2,
                y: Position.y + Size.y / 2 + 13,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
        }
    }
}
