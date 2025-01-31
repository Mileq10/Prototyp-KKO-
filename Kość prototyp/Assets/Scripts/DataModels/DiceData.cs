namespace DataModels
{
    [System.Serializable]
    public class DiceData
    {
        public EDiceSide Side { get; set; }
        
    
        public DiceData(EDiceSide side)
        {
            Side = side;
        }
    }
}