namespace DecisionTree.Tests.Dto
{
    public class BoolDto
    {
        public BoolDto(bool value)
        {
            BoolProperty = value;
        }
        public bool BoolProperty { get; set; }
        public bool Result { get; set; }

        public BoolDto SetResult(bool value)
        {
            Result = value;
            return this;
        }
    }
}
