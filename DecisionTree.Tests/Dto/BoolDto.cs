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
        public bool ActionFlag { get; set; }

        public BoolDto SetResult(bool value)
        {
            Result = value;
            return this;
        }

        public BoolDto DoSomeAction()
        {
            ActionFlag = true;
            return this;
        }
    }
}
