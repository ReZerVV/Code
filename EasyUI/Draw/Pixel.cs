namespace EasyUI.Draw
{
    public class Pixel
    {
        public string Symbol { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }

        public Pixel(string symbol = " ")
        {
            Symbol = symbol;
            Foreground = new Color(255, 255, 255);
            Background = new Color(0, 0, 0);
        }
    }
}
