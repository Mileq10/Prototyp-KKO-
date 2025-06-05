namespace DataModels
{
    [System.Serializable]
    public class DiceData
    {
        public EDiceSide Side { get; set; }

        public override string ToString()
        {
            return Side.ToString();
        }

        public DiceData(EDiceSide side)
        {
            Side = side;
        }
    }
}