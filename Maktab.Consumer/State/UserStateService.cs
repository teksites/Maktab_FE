using Maktab.Consumer.State.Course;
using Maktab.Consumer.State.Parent;

namespace Maktab.Consumer.State
{
     public class UserStateService
     {
          public ParentProfileState ParentState { get; set; } = new();
          public InstituteState InstituteState { get; set; } = new();
     }
}
