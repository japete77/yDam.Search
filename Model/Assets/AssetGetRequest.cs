namespace yDam.Dam.Model.Assets
{
    public class AssetGetRequest
    {        
        public string SearchText { get; set; }        
        public int MaxResults { get; set; }
        public int Skip { get; set; }
    }
}