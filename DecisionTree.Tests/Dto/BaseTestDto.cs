namespace DecisionTree.Tests.Dto
{
    public class BaseTestDto
    {
        public bool ActionFlag { get; set; }

        public BaseTestDto DoSomeAction()
        {
            ActionFlag = true;
            return this;
        }
    }
}
