#if ENABLE_PERFORMANCE_TESTS
using UnityEngine;
using UnityEngine.TestTools;

public class ExistingMonobehaviourTest<T> : CustomYieldInstruction where T : MonoBehaviour, IMonoBehaviourTest
{
    public T component { get; private set; }

    public ExistingMonobehaviourTest(T component)
    {
        this.component = component;
    }

    public override bool keepWaiting
    {
        get { return !component.IsTestFinished; }
    }
}
#endif