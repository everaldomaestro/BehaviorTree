// See https://aka.ms/new-console-template for more information
using BehaviorTree.BT;
using static BehaviorTree.BT.Node;

int count = 0;

var sequence01 = new Sequence("Sequence 01");

var leafInverter01 = new Leaf("Sequence 01 - Inverter 01 - Leaf 01", TYPE.CONDITION, Failure);
var inverter01 = new Inverter("Sequence 01 - Inverter 01");
inverter01.AddChild(leafInverter01);
sequence01.AddChild(inverter01);

var fallback01 = new Fallback("Sequence 01 - Fallback 01");
var leafFallback01 = new Leaf("Sequence 01 - Fallback 01 - Leaf 01", TYPE.ACTION, CounterThatWillFail);
var leafFallback02 = new Leaf("Sequence 01 - Fallback 01 - Leaf 02", TYPE.ACTION, Success);
fallback01.AddChild(leafFallback01);
fallback01.AddChild(leafFallback02);
sequence01.AddChild(fallback01);

var retry01 = new Retry("Sequence 01 - Retry 01", 10);
var leafRetry01 = new Leaf("Sequence 01 - Retry 01 - Leaf 01", TYPE.ACTION, CounterFailAndSuccess);
retry01.AddChild(leafRetry01);
sequence01.AddChild(retry01);

var leafSequence01 = new Leaf("Sequence 01 - Leaf 01", TYPE.ACTION, CounterThatWillNotFail);
var leafSequence02 = new Leaf("Sequence 01 - Leaf 02", TYPE.ACTION, CounterThatWillNotFail);
var leafSequence03 = new Leaf("Sequence 01 - Leaf 03", TYPE.ACTION, CounterThatWillNotFail);
sequence01.AddChild(leafSequence01);
sequence01.AddChild(leafSequence02);
sequence01.AddChild(leafSequence03);

while (sequence01.GetStatus() == STATUS.RUNNING)
{
    sequence01.Process();
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
