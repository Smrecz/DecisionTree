namespace DecisionTree.Tests.Dto
{
    public class IntDto : BaseTestDto
    {
        public IntDto(int value)
        {
            IntProperty = value;
        }
        public int IntProperty { get; set; }
        public int Result { get; set; }

        public IntDto SetResult(int value)
        {
            Result = value;
            return this;
        }
    }
}
