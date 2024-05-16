namespace Sopra.Maps
{
    public class RiddleList
    {
        private int mRiddleItem1;
        private int mRiddleItem2;
        private string mKeyword;
        private bool mCheck;
        public RiddleList(string keyword, int riddleItem1, int riddleItem2,bool check)
        {
            mRiddleItem1 = riddleItem1;
            mRiddleItem2 = riddleItem2;
            mKeyword = keyword;
            mCheck = check;

        }

        public int GetRiddleItem1()
        {

            return mRiddleItem1;
        }
        public int GetRiddleItem2()
        {

            return mRiddleItem2;
        }
        public string GetKeyword()
        {

            return mKeyword;
        }
        public bool GetCheck()
        {

            return mCheck;
        }

        public void SetRiddleItem1(int riddleItem1)
        {
            mRiddleItem1 = riddleItem1;

        }
        public void SetRiddleItem2(int riddleItem2)
        {
            mRiddleItem2 = riddleItem2;

        }
        public void SetRiddleCheck(bool check)
        {
            mCheck = check;

        }


    }
}
