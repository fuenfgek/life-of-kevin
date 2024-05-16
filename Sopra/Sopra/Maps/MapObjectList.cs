namespace Sopra.Maps
{
    public class MapObjectList
    {
        private string mName;
        private string mType;
        private double mX;
        private double mY;
        private int mWidth;
        private int mHeight;

        public MapObjectList(string name, string type, double x, double y, int width, int height)
        {
            mName = name;
            mType = type;
            mX = x;
            mY = y;
            mWidth = width;
            mHeight = height;

        }

        public string GetName()
        {
            return mName;
        }

        public string GetType()
        {
            return mType;
        }

        public double GetX()
        {
            return mX;
        }

        public double GetY()
        {
            return mY;
        }

        public int GetWidth()
        {
            return mWidth;
        }

        public int GetHeight()
        {
            return mHeight;
        }
    }
}
