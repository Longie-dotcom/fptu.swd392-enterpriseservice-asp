namespace Domain.Entity
{
    public class Member
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid MemberID { get; private set; }
        public Guid UserID { get; private set; } // Map with IAM Service
        public DateTime AssignedAt { get; private set; }
        public DateTime UnassignedAt { get; private set; }

        public Guid EnterpriseID { get; private set; }
        #endregion

        protected Member() { }

        public Member(
            Guid memberId,
            Guid enterpriseId,
            Guid userId)
        {
            MemberID = memberId;
            EnterpriseID = enterpriseId;
            UserID = userId;
            AssignedAt = DateTime.UtcNow;
        }

        #region Methods
        public void Unassigned()
        {
            UnassignedAt = DateTime.UtcNow;
        }
        #endregion
    }
}
