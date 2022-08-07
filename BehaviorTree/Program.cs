// See https://aka.ms/new-console-template for more information
using BehaviorTree.BT;
using BehaviorTree.BT.Interfaces;
using static BehaviorTree.BT.Node;
using static BehaviorTree.BT.CompositeNodes.SyncParallelNode;

int count = 0;

INode tree;

var builder = new BehaviourTreeBuilder();

tree = builder
        .Root("Root", true)
            .RandomSequence("Sequence 01")
                .Inverter("Sequence 01 - Inverter 01")
                    .Condition("Sequence 01 - Inverter 01 - Leaf 01", Failure)
                .End()
                .Fallback("Sequence 01 - Fallback 01")
                    .Action("Sequence 01 - Fallback 01 - Leaf 01", CounterThatWillFail)
                    .Action("Sequence 01 - Fallback 01 - Leaf 02", Success)
                .End()
                .Retry("Sequence 01 - Retry 01", 10)
                    .Action("Sequence 01 - Retry 01 - Leaf 01", CounterFailAndSuccess)
                .End()
                .Sequence("Sequence 01 - Sequence 02")
                    .Action("Sequence 01 - Sequence 02 - Leaf 01", CounterThatWillNotFail)
                    .Action("Sequence 01 - Sequence 02 - Leaf 02", CounterThatWillNotFail)
                    .Action("Sequence 01 - Sequence 02 - Leaf 03", CounterThatWillNotFail)
                .End()
                .SyncParallel("Sequence 01 - Parallel 01", POLICY.REQUIRE_ALL, POLICY.REQUIRE_ONE, STATUS.FAILURE)
                    .Action("Sequence 01 - Parallel 01 - Leaf 01", CounterThatWillNotFail)
                    .Action("Sequence 01 - Parallel 01 - Leaf 02", CounterThatWillNotFail)
                .End()
            .End()
        .End()
        .Build();

while (tree.GetStatus() == STATUS.RUNNING)
{
    tree.Tick();
    Console.WriteLine("---------------------------------------------------------------");
}

STATUS CounterThatWillNotFail()
{
    count++;
    if (count <= 10)
        return STATUS.RUNNING;

    count = 0;
    return STATUS.SUCCESS;
}

STATUS CounterThatWillFail()
{
    count++;
    if (count <= 10)
        return STATUS.RUNNING;

    count = 0;
    return STATUS.FAILURE;
}

STATUS CounterFailAndSuccess()
{
    count++;
    if (count < 10)
        return STATUS.FAILURE;

    count = 0;
    return STATUS.SUCCESS;
}

STATUS Success()
{
    return STATUS.SUCCESS;
}

STATUS Failure()
{
    return STATUS.FAILURE;
}
