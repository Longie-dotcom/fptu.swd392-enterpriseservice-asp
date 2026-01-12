using Domain.Enum;

namespace Domain.Entity
{
    public class Capacity
    {
        #region Attributes
        private readonly List<CollectionAssignment> collectionAssignments = new List<CollectionAssignment>();
        #endregion

        #region Properties
        public Guid CapacityID { get; private set; }
        public double MaxDailyCapacity { get; private set; }
        public string RegionCode { get; private set; }
        public UnitOfMeasure UnitOfMeasure { get; private set; }
        public double CurrentLoad { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ClosedAt { get; private set; }

        public IReadOnlyCollection<CollectionAssignment> CollectionAssignments
        {
            get { return collectionAssignments.AsReadOnly(); }
        }

        public string WasteType { get; private set; }
        public Guid EnterpriseID { get; private set; }
        #endregion

        protected Capacity() { }

        public Capacity(
            Guid capacityId, 
            Guid enterpriseId, 
            UnitOfMeasure unitOfMeasure,
            string regionCode,
            double maxDailyCapacity)
        {
            CapacityID = capacityId;
            EnterpriseID = enterpriseId;
            MaxDailyCapacity = maxDailyCapacity;
            RegionCode = regionCode;
            UnitOfMeasure = unitOfMeasure;
            CurrentLoad = 0;
            CreatedAt = DateTime.UtcNow;
        }

        #region Methods
        public void Close()
        {
            ClosedAt = DateTime.UtcNow;
        }
        #endregion
    }
}
