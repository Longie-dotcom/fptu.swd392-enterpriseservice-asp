namespace Domain.Aggregate
{
    public class WasteType
    {
        #region Attributes
        #endregion

        #region Properties
        public string Type { get; private set; }
        public string Description { get; private set; }
        #endregion

        protected WasteType() { }

        public WasteType(
            string type, 
            string description)
        {
            Type = type;
            Description = description;
        }

        #region Methods
        #endregion
    }
}
