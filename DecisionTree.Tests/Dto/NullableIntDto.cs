namespace DecisionTree.Tests.Dto
{
    public class NullableIntDto : BaseTestDto
    {
        public NullableIntDto(int? value)
        {
            IntProperty = value;
        }
        public int? IntProperty { get; set; }
        public int? Result { get; set; }

        public NullableIntDto SetResult(int? value)
        {
            Result = value;
            return this;
        }
    }
}
