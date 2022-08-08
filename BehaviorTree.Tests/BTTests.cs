using BehaviorTree.BT;
using BehaviorTree.BT.Interfaces;
using static BehaviorTree.BT.Abstract.Node;
using static BehaviorTree.BT.CompositeNodes.SyncParallelNode;

namespace BehaviorTree.Tests
{
    public class BTTests
    {
        int count = 0;
        int i = 0;

        INode? tree;
        private readonly BehaviourTreeBuilder builder = new();

        [Fact]
        public void CreateSimpleBT()
        {
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            tree.Tick();

            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void CreateSimpleBT_STATUS_SUCCESS()
        {
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            tree.Tick();

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void CreateSimpleBT_STATUS_FAILURE()
        {
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Failure)
                            .End()
                        .End()
                        .Build();

            tree.Tick();

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void CreateSimpleBT_STATUS_RUNNING()
        {
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Running)
                            .End()
                        .End()
                        .Build();

            tree.Tick();

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void CreateSimpleBT_STATUS_RUNNING_IN_LOOP()
        {
            i = 0;
            tree = builder
                        .Root("Root")
                            .Sequence("Sequence")
                                .Action("Action", Failure)
                            .End()
                        .End()
                        .Build();

            while (i < 5)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Sequence_STATUS_FAILURE()
        {
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Success)
                                .Action("Action", Failure)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 2);
            Assert.True(tree.HasChildren());

            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Failure)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 4);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Sequence_STATUS_SUCCESS()
        {
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 5);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Sequence_STATUS_RUNNING()
        {
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Running)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (i < 5)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());

            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Sequence("Sequence")
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Running)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (i < 5)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());

            i = 0;
            tree = builder
                        .Root("Root")
                            .Sequence("Sequence")
                                .Action("Action", Running)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (i < 5)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Fallback_STATUS_SUCCESS()
        {
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 1);
            Assert.True(tree.HasChildren());

            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 5);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Fallback_STATUS_FAILURE()
        {
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 1);
            Assert.True(tree.HasChildren());

            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 5);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Fallback_STATUS_RUNNING()
        {
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Running)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (i < 5)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());

            i = 0;
            tree = builder
                        .Root("Root")
                            .Fallback("Fallback")
                                .Action("Action", Running)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (i < 5)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void SyncParallel_STATUS_SUCCESS()
        {
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ALL, POLICY.REQUIRE_ONE, STATUS.FAILURE)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 1);
            Assert.True(tree.HasChildren());

            count = 0;
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ALL, POLICY.REQUIRE_ONE, STATUS.FAILURE)
                                .Action("Action", CounterThatWillNotFail)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 5);
            Assert.True(tree.HasChildren());

            count = 0;
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ONE, POLICY.REQUIRE_ONE, STATUS.FAILURE)
                                .Action("Action", CounterThatWillNotFail)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 1);
            Assert.True(tree.HasChildren());

            count = 0;
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ALL, POLICY.REQUIRE_ALL, STATUS.SUCCESS)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 2);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void SyncParallel_STATUS_FAILURE()
        {
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ALL, POLICY.REQUIRE_ONE, STATUS.SUCCESS)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Failure)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 1);
            Assert.True(tree.HasChildren());

            count = 0;
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ALL, POLICY.REQUIRE_ONE, STATUS.SUCCESS)
                                .Action("Action", CounterThatWillFail)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 5);
            Assert.True(tree.HasChildren());

            count = 0;
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ONE, POLICY.REQUIRE_ALL, STATUS.SUCCESS)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 1);
            Assert.True(tree.HasChildren());

            count = 0;
            i = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ALL, POLICY.REQUIRE_ALL, STATUS.FAILURE)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Failure)
                                .Action("Action", Failure)
                                .Action("Action", Success)
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 2);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void SyncParallel_STATUS_RUNNING()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .SyncParallel("SyncParallel", POLICY.REQUIRE_ALL, POLICY.REQUIRE_ONE, STATUS.SUCCESS)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Success)
                                .Action("Action", Running)
                            .End()
                        .End()
                        .Build();

            while (i < 10)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }


        [Fact]
        public void ForceFailure_STATUS_FAILURE()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .ForceFailure("ForceFailure")
                                    .Action("Action", Success)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 6);
            Assert.True(tree.HasChildren());

            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .ForceFailure("ForceFailure")
                                    .Action("Action", Failure)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 6);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void ForceFailure_STATUS_RUNNING()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .ForceFailure("ForceFailure")
                                    .Action("Action", Running)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (i < 10)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void ForceSuccess_STATUS_SUCCESS()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .ForceSuccess("ForceSuccess")
                                    .Action("Action", Failure)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 6);
            Assert.True(tree.HasChildren());

            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .ForceSuccess("ForceSuccess")
                                    .Action("Action", Success)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 6);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void ForceSuccess_STATUS_RUNNING()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .ForceSuccess("ForceSuccess")
                                    .Action("Action", Running)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (i < 10)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Inverter_STATUS_RUNNING()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .Inverter("Inverter")
                                    .Action("Action", Running)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (i < 10)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Inverter_STATUS_SUCCESS()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .Inverter("Inverter")
                                    .Action("Action", Failure)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Inverter_STATUS_FAILURE()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", CounterThatWillFail)
                                .Inverter("Inverter")
                                    .Action("Action", Success)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Repeat_STATUS_RUNNING()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Repeat("Repeat", 10)
                                    .Action("Action", Success)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (i < 7)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Repeat_STATUS_SUCCESS()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Repeat("Repeat", 10)
                                    .Action("Action", Success)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 11);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Repeat_STATUS_FAILURE()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Repeat("Repeat", 10)
                                    .Action("Action", CounterSuccessAnFail)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 6);
            Assert.True(tree.HasChildren());

            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Repeat("Repeat", 10)
                                    .Action("Action", Failure)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 2);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Retry_STATUS_RUNNING()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Retry("Retry", 10)
                                    .Action("Action", Failure)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (i < 7)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.RUNNING);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Retry_STATUS_SUCCESS()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Retry("Retry", 10)
                                    .Action("Action", CounterFailAndSuccess)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 6);
            Assert.True(tree.HasChildren());

            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Retry("Retry", 10)
                                    .Action("Action", Success)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.SUCCESS);
            Assert.True(i == 2);
            Assert.True(tree.HasChildren());
        }

        [Fact]
        public void Retry_STATUS_FAILURE()
        {
            i = 0;
            count = 0;
            tree = builder
                        .Root("Root", false)
                            .Fallback("Fallback")
                                .Action("Action", Failure)
                                .Retry("Retry", 10)
                                    .Action("Action", Failure)
                                .End()
                            .End()
                        .End()
                        .Build();

            while (tree.GetStatus() == STATUS.RUNNING)
            {
                i++;
                tree.Tick();
            }

            Assert.True(tree.GetStatus() == STATUS.FAILURE);
            Assert.True(i == 11);
            Assert.True(tree.HasChildren());
        }

        STATUS CounterThatWillNotFail()
        {
            count++;
            if (count < 5) return Running();

            count = 0;
            return Success();
        }

        STATUS CounterThatWillFail()
        {
            count++;
            if (count < 5) return Running();

            count = 0;
            return Failure();
        }

        STATUS CounterFailAndSuccess()
        {
            count++;
            if (count < 5) return Failure();

            count = 0;
            return Success();
        }

        STATUS CounterSuccessAnFail()
        {
            count++;
            if (count < 5) return Success();

            count = 0;
            return Failure();
        }

        STATUS Success() => STATUS.SUCCESS;

        STATUS Failure() => STATUS.FAILURE;

        STATUS Running() => STATUS.RUNNING;
    }
}