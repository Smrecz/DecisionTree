using System;

namespace DecisionTree.Tests.Dto
{
    public class ItProjectDecisionDtoWithError : ItProjectDecisionDto
    {
        public override ItProjectDecisionDto SetIsOnHold(bool value)
        {
            throw new ArgumentException("This is test exception!");
        }
    }
}
