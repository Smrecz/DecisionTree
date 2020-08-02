namespace DecisionTree.Tests.Dto
{
    public class BoolDto
    {
        public BoolDto(bool value)
        {
            BoolProperty = value;
        }
        public bool BoolProperty { get; }
        public bool Result { get; set; }
    }
}
