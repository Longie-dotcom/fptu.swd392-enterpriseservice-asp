using Domain.Enum;

namespace Domain.Entity
{
    public class CollectionAssignment
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid CollectionAssignmentID { get; private set; }
        public Guid CollectionReportID { get; private set; } // Map with Citizen Service
        public Guid AssigneeID { get; private set; } // Map with Collection Service
        public string Note { get; private set; }
        public PriorityLevel PriorityLevel { get; private set; }
        public DateTime AcceptedAt { get; private set; }
        public CollectionReportStatus Status { get; private set; }

        public Guid CapacityID { get; private set; }
        #endregion

        protected CollectionAssignment() { }

        public CollectionAssignment(
            Guid collectionAssignmentId,
            Guid collectionReportId,
            Guid capacityID,
            Guid assigneeId,
            string note,
            PriorityLevel priorityLevel)
        {
            CollectionAssignmentID = collectionAssignmentId;
            CollectionReportID = collectionReportId;
            CapacityID = capacityID;
            AssigneeID = assigneeId;
            Note = note;
            PriorityLevel = priorityLevel;
            AcceptedAt = DateTime.Now;
            Status = CollectionReportStatus.Assigned;
        }

        #region Methods
        #endregion
    }
}
