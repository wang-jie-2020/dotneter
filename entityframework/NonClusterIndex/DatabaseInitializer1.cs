using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Demo
{
    public class DatabaseInitializer1
    {
        public static void Seed(IApplicationBuilder app)
        {
            var wipOrderCommands1 = new List<string>();

            int index1 = 241105;
            int index2 = index1 + 200;  //242834

            for (var i = index1; i < index2; i++)
            {
                wipOrderCommands1.Add($"UPDATE [dbo].[WipOrder_DoNotUse] SET [WorkOrderStatus] = N'待测' WHERE WipOrderId = {i}");
            }

            var wipOperationCommands1 = new List<string>();
            for (var i = index1; i < index2; i++)
            {
                wipOperationCommands1.Add($"UPDATE [dbo].[WipOperation_DoNotUse] SET [OperationStatus] = N'已排程' WHERE WipOrderId = {i}");
            }

            var wipOrderCommands2 = new List<string>();
            for (var i = index1; i < index2; i++)
            {
                wipOrderCommands2.Add($"UPDATE [dbo].[WipOrder_DoNotUse] SET [WorkOrderStatus] = N'在测' WHERE WipOrderId = {i}");
            }

            var wipOperationCommands2 = new List<string>();
            for (var i = index1; i < index2; i++)
            {
                wipOperationCommands2.Add($"UPDATE [dbo].[WipOperation_DoNotUse] SET [OperationStatus] = N'未排程' WHERE WipOrderId = {i}");
            }

            Parallel.For(0, 2, (i, pls) =>
            {
                Task.Run(async () =>
                {
                    using (var services = app.ApplicationServices.CreateScope())
                    {
                        var context = services.ServiceProvider.GetRequiredService<AppDbContext>();

                        while (true)
                        {
                            try
                            {
                                var count = await context.Database.ExecuteSqlRawAsync(querySQL);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            await Task.Delay(1000);
                        }
                    }
                });

                Task.Run(async () =>
                {
                    using (var services = app.ApplicationServices.CreateScope())
                    {
                        var context = services.ServiceProvider.GetRequiredService<AppDbContext>();

                        while (true)
                        {
                            try
                            {
                                var count = await context.Database.ExecuteSqlRawAsync(querySQL);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                            await Task.Delay(1000);
                        }
                    }
                });
            });
            Parallel.For(0, 2, (i, pls) =>
            {
                Task.Run(async () =>
                {
                    using (var services = app.ApplicationServices.CreateScope())
                    {
                        var context = services.ServiceProvider.GetRequiredService<AppDbContext>();

                        while (true)
                        {
                            using (var trans = await context.Database.BeginTransactionAsync())
                            {
                                try
                                {
                                    foreach (var command in wipOrderCommands1)
                                    {
                                        await context.Database.ExecuteSqlRawAsync(command);
                                    }

                                    foreach (var command in wipOperationCommands1)
                                    {
                                        await context.Database.ExecuteSqlRawAsync(command);
                                    }

                                    await context.Database.ExecuteSqlRawAsync(insertSQL1);

                                    await trans.CommitAsync();
                                }
                                catch (Exception e)
                                {
                                    //Console.WriteLine(e.Message);
                                    await trans.RollbackAsync();
                                }
                            }

                            await Task.Delay(1000);
                        }
                    }
                });
            });
            Parallel.For(0, 2, (i, pls) =>
            {
                Task.Run(async () =>
                {
                    using (var services = app.ApplicationServices.CreateScope())
                    {
                        var context = services.ServiceProvider.GetRequiredService<AppDbContext>();

                        while (true)
                        {
                            using (var trans = await context.Database.BeginTransactionAsync())
                            {
                                try
                                {
                                    foreach (var command in wipOrderCommands2)
                                    {
                                        await context.Database.ExecuteSqlRawAsync(command);
                                    }

                                    foreach (var command in wipOperationCommands2)
                                    {
                                        await context.Database.ExecuteSqlRawAsync(command);
                                    }

                                    await context.Database.ExecuteSqlRawAsync(insertSQL2);

                                    await trans.CommitAsync();
                                }
                                catch (Exception e)
                                {
                                    //Console.WriteLine(e.Message);
                                    await trans.RollbackAsync();
                                }
                            }

                            await Task.Delay(1000);
                        }
                    }
                });
            });
        }

        private const string querySQL = @"
            SELECT
             [t2].[InventoryId],
             [t2].[Active],
             [t2].[ActualChannelNumber],
             [t2].[Applicant],
             [t2].[BarCode],
             [t2].[CheckState],
             [t2].[ConfirmedBy],
             [t2].[ConfirmedTime],
             [t2].[Container],
             [t2].[CreateDate],
             [t2].[CreateID],
             [t2].[Creator],
             [t2].[CurrentWipOperationId],
             [t2].[DelayRemark],
             [t2].[DiversityExplain],
             [t2].[DiversityReason],
             [t2].[GetSampleBy],
             [t2].[GetSampleTime],
             [t2].[Hazardlevel],
             [t2].[ISDiversity],
             [t2].[Inspector],
             [t2].[InspectorTime],
             [t2].[InventoryStatus],
             [t2].[InventoryTestStatus],
             [t2].[InventoryType],
             [t2].[IsChangCupboard],
             [t2].[IsDeleted],
             [t2].[IsOrderSparePart],
             [t2].[IsReplaced],
             [t2].[IsScrapped],
             [t2].[LotNo],
             [t2].[Modifier],
             [t2].[ModifyDate],
             [t2].[ModifyID],
             [t2].[OrderHeaderId],
             [t2].[OrderSampleId],
             [t2].[PartnerID],
             [t2].[PendingRemark],
             [t2].[ProductID],
             [t2].[Quantity],
             [t2].[RealityManage],
             [t2].[ReceiptTime],
             [t2].[Recipient],
             [t2].[Remark],
             [t2].[ReturnReason],
             [t2].[SampleACR],
             [t2].[SampleOCV],
             [t2].[SampleStatus],
             [t2].[ScrapRemark],
             [t2].[SerialNo],
             [t2].[TemporaryManage],
             [t2].[TestNo],
             [t2].[UPCabinetStatus],
             [t2].[WarehouseID],
             [t2].[WarehouseLocationID],
             [t3].[OrderHeaderID],
             [t3].[ActualCost],
             [t3].[Applicant],
             [t3].[ApplicantDep],
             [t3].[ApplicantId],
             [t3].[Attachment],
             [t3].[AuditRemark],
             [t3].[CreateDate],
             [t3].[CreateID],
             [t3].[Creator],
             [t3].[DraftStatus],
             [t3].[EstimatedCost],
             [t3].[IsCAEAssessment],
             [t3].[IsCAEProject],
             [t3].[IsDeleted],
             [t3].[IsProductCodeHand],
             [t3].[IsRisk],
             [t3].[IsUrgent],
             [t3].[Modifier],
             [t3].[ModifyDate],
             [t3].[ModifyID],
             [t3].[OAUniqueIdentification],
             [t3].[OrderDate],
             [t3].[OrderHerNo],
             [t3].[OrderStatus],
             [t3].[OrderType],
             [t3].[PackageId],
             [t3].[ProcessClass],
             [t3].[ProcessType],
             [t3].[ProductPN],
             [t3].[ProjectCode],
             [t3].[ProjectName],
             [t3].[ProjectShortCode],
             [t3].[ProjectShortName],
             [t3].[Quantity],
             [t3].[ReceivedQuantity],
             [t3].[Remark],
             [t3].[RequireDescription],
             [t3].[SampleDescribe],
             [t3].[SampleGroup],
             [t3].[SampleSource],
             [t3].[SampleSpec],
             [t3].[SampleSpecName],
             [t3].[SampleStage],
             [t3].[SampleType],
             [t3].[SampleTypeName],
             [t3].[SampleVersion],
             [t3].[SimulationReport],
             [t3].[TaskPeriod],
             [t3].[TaskProject],
             [t3].[TestClass],
             [t3].[TestPurposes],
             [t3].[TestStatus],
             [t4].[WipOperationID],
             [t4].[ATestCode],
             [t4].[ActualChannelNumber],
             [t4].[ActualCompletionDate],
             [t4].[ActualDurationSeconds],
             [t4].[ActualLocationId],
             [t4].[ActualStartDate],
             [t4].[AssignTime],
             [t4].[BarCode],
             [t4].[ChannelNumber],
             [t4].[CreateDate],
             [t4].[CreateID],
             [t4].[Creator],
             [t4].[DefaultSamplingTime],
             [t4].[Description],
             [t4].[Description1],
             [t4].[DiversityExplain],
             [t4].[DiversityReason],
             [t4].[DownLoadBopBy],
             [t4].[DownLoadBopById],
             [t4].[ExpectedCompletionDate],
             [t4].[ExpectedDurationSeconds],
             [t4].[ExpectedStartDate],
             [t4].[Handler],
             [t4].[HandlerIds],
             [t4].[IsDeleted],
             [t4].[IsDiversity],
             [t4].[IsInsert],
             [t4].[LocationId],
             [t4].[Modifier],
             [t4].[ModifyDate],
             [t4].[ModifyID],
             [t4].[OperationCode],
             [t4].[OperationId],
             [t4].[OperationName],
             [t4].[OperationStatus],
             [t4].[Operator],
             [t4].[OperatorId],
             [t4].[OprSequenceNo],
             [t4].[OrderHeaderId],
             [t4].[Position],
             [t4].[ProcessId],
             [t4].[ProcessOperationId],
             [t4].[Remark],
             [t4].[Result],
             [t4].[ScheduledCompletionDate],
             [t4].[ScheduledDurationSeconds],
             [t4].[ScheduledStartDate],
             [t4].[TestNo],
             [t4].[WipOperationAssignId],
             [t4].[WipOrderID],
             [t2].[WipOperationID],
             [t2].[ATestCode],
             [t2].[ActualChannelNumber0],
             [t2].[ActualCompletionDate],
             [t2].[ActualDurationSeconds],
             [t2].[ActualLocationId],
             [t2].[ActualStartDate],
             [t2].[AssignTime],
             [t2].[BarCode0],
             [t2].[ChannelNumber],
             [t2].[CreateDate0],
             [t2].[CreateID0],
             [t2].[Creator0],
             [t2].[DefaultSamplingTime],
             [t2].[Description],
             [t2].[Description1],
             [t2].[DiversityExplain0],
             [t2].[DiversityReason0],
             [t2].[DownLoadBopBy],
             [t2].[DownLoadBopById],
             [t2].[ExpectedCompletionDate],
             [t2].[ExpectedDurationSeconds],
             [t2].[ExpectedStartDate],
             [t2].[Handler],
             [t2].[HandlerIds],
             [t2].[IsDeleted0],
             [t2].[IsDiversity0],
             [t2].[IsInsert],
             [t2].[LocationId],
             [t2].[Modifier0],
             [t2].[ModifyDate0],
             [t2].[ModifyID0],
             [t2].[OperationCode],
             [t2].[OperationId],
             [t2].[OperationName],
             [t2].[OperationStatus],
             [t2].[Operator],
             [t2].[OperatorId],
             [t2].[OprSequenceNo],
             [t2].[OrderHeaderId0],
             [t2].[Position],
             [t2].[ProcessId],
             [t2].[ProcessOperationId],
             [t2].[Remark0],
             [t2].[Result],
             [t2].[ScheduledCompletionDate],
             [t2].[ScheduledDurationSeconds],
             [t2].[ScheduledStartDate],
             [t2].[TestNo0],
             [t2].[WipOperationAssignId],
             [t2].[WipOrderID],
             [t5].[WipOrderID],
             [t5].[ACRExtentVmax],
             [t5].[ACRExtentVmin],
             [t5].[ActualCompletionDate],
             [t5].[ActualDurationSeconds],
             [t5].[ActualStartDate],
             [t5].[BarCode],
             [t5].[ChannelNumber],
             [t5].[CompletedQuantity],
             [t5].[ConfirmedBy],
             [t5].[ConfirmedDatetime],
             [t5].[CreateDate],
             [t5].[CreateID],
             [t5].[Creator],
             [t5].[Duration],
             [t5].[ExpectedCompletionDate],
             [t5].[ExpectedDurationSeconds],
             [t5].[ExpectedStartDate],
             [t5].[IsDeleted],
             [t5].[Length],
             [t5].[LotNo],
             [t5].[Modifier],
             [t5].[ModifyDate],
             [t5].[ModifyID],
             [t5].[OrderDetailId],
             [t5].[OrderHeaderId],
             [t5].[OrderLineNo],
             [t5].[OrderNo],
             [t5].[OrderProcessId],
             [t5].[OrderQuantity],
             [t5].[Priority],
             [t5].[ProcessID],
             [t5].[ProcessName],
             [t5].[ProductCode],
             [t5].[ProductID],
             [t5].[RatedCapacity],
             [t5].[RatedPower],
             [t5].[RealityManage],
             [t5].[ReceivedDate],
             [t5].[Remark],
             [t5].[Remarks],
             [t5].[ScheduledCompletionDate],
             [t5].[ScheduledDurationSeconds],
             [t5].[ScheduledStartDate],
             [t5].[SerialNo],
             [t5].[Spec],
             [t5].[SpecialFixture],
             [t5].[SpecialTemp],
             [t5].[TemporaryManage],
             [t5].[TemporaryTime],
             [t5].[TestNo],
             [t5].[ThicknessVmax],
             [t5].[ThicknessVmin],
             [t5].[Vmax],
             [t5].[Vmin],
             [t5].[WarehouseId],
             [t5].[WeightVmax],
             [t5].[WeightVmin],
             [t5].[Width],
             [t5].[WipOrderGroupId],
             [t5].[WipSequenceNo],
             [t5].[WorkOrderStatus],
             [t6].[WipOrderGroupId],
             [t6].[ACRExtentVmax],
             [t6].[ACRExtentVmin],
             [t6].[CreateDate],
             [t6].[CreateID],
             [t6].[Creator],
             [t6].[IsDeleted],
             [t6].[Length],
             [t6].[Modifier],
             [t6].[ModifyDate],
             [t6].[ModifyID],
             [t6].[OrderDetailId],
             [t6].[OrderHeaderId],
             [t6].[OrderQuantity],
             [t6].[ProcessID],
             [t6].[ProcessName],
             [t6].[ProductCode],
             [t6].[ProductID],
             [t6].[ProductName],
             [t6].[RatedCapacity],
             [t6].[RatedPower],
             [t6].[Remark],
             [t6].[Spec],
             [t6].[ThicknessVmax],
             [t6].[ThicknessVmin],
             [t6].[Vmax],
             [t6].[Vmin],
             [t6].[WeightVmax],
             [t6].[WeightVmin],
             [t6].[Width],
             [t7].[OrderDetailId],
             [t7].[Attachment],
             [t7].[BopWorkTime],
             [t7].[CreateDate],
             [t7].[CreateID],
             [t7].[Creator],
             [t7].[DescriptionOne],
             [t7].[FixtureAsk],
             [t7].[IsDeleted],
             [t7].[Modifier],
             [t7].[ModifyDate],
             [t7].[ModifyID],
             [t7].[OrderHeaderID],
             [t7].[OrderLineNo],
             [t7].[OrderProcessId],
             [t7].[OrderStatus],
             [t7].[ProcessClassName],
             [t7].[ProcessID],
             [t7].[ProcessName],
             [t7].[ProcessQuantity],
             [t7].[SampleSpecName],
             [t7].[Sign],
             [t7].[SpecialFixture],
             [t7].[TaskName],
             [t7].[TaskPeriod],
             [t7].[TaskProject],
             [t7].[TaskProjectName],
             [t7].[TaskStandard],
             [t7].[TaskType],
             [t7].[TaskTypeName],
             [t8].[OrderHeaderID],
             [t8].[ActualCost],
             [t8].[Applicant],
             [t8].[ApplicantDep],
             [t8].[ApplicantId],
             [t8].[Attachment],
             [t8].[AuditRemark],
             [t8].[CreateDate],
             [t8].[CreateID],
             [t8].[Creator],
             [t8].[DraftStatus],
             [t8].[EstimatedCost],
             [t8].[IsCAEAssessment],
             [t8].[IsCAEProject],
             [t8].[IsDeleted],
             [t8].[IsProductCodeHand],
             [t8].[IsRisk],
             [t8].[IsUrgent],
             [t8].[Modifier],
             [t8].[ModifyDate],
             [t8].[ModifyID],
             [t8].[OAUniqueIdentification],
             [t8].[OrderDate],
             [t8].[OrderHerNo],
             [t8].[OrderStatus],
             [t8].[OrderType],
             [t8].[PackageId],
             [t8].[ProcessClass],
             [t8].[ProcessType],
             [t8].[ProductPN],
             [t8].[ProjectCode],
             [t8].[ProjectName],
             [t8].[ProjectShortCode],
             [t8].[ProjectShortName],
             [t8].[Quantity],
             [t8].[ReceivedQuantity],
             [t8].[Remark],
             [t8].[RequireDescription],
             [t8].[SampleDescribe],
             [t8].[SampleGroup],
             [t8].[SampleSource],
             [t8].[SampleSpec],
             [t8].[SampleSpecName],
             [t8].[SampleStage],
             [t8].[SampleType],
             [t8].[SampleTypeName],
             [t8].[SampleVersion],
             [t8].[SimulationReport],
             [t8].[TaskPeriod],
             [t8].[TaskProject],
             [t8].[TestClass],
             [t8].[TestPurposes],
             [t8].[TestStatus],
             [t2].[WipOrderID0],
             [t2].[OperationId0],
             [t9].[InventoryErrorId],
             [t9].[AttachmentInformationIDs],
             [t9].[AttachmentName],
             [t9].[BarCode],
             [t9].[Comment],
             [t9].[ConfirmAt],
             [t9].[ConfirmBy],
             [t9].[ConfirmId],
             [t9].[CreateDate],
             [t9].[CreateID],
             [t9].[Creator],
             [t9].[CurrentState],
             [t9].[ErrorRemark],
             [t9].[ErrorType],
             [t9].[Feedbacktime],
             [t9].[FileName],
             [t9].[InventoryId],
             [t9].[IsDeleted],
             [t9].[IsExplosionProofBox],
             [t9].[Modifier],
             [t9].[ModifyDate],
             [t9].[ModifyID],
             [t9].[TestDuration],
             [t9].[TestTemperature],
             [t9].[UnpackAbnormalRecordDetailID],
             [t9].[WarehouseID],
             [t9].[WarehouseLocationID] 
            FROM
             (
             SELECT
              [i].[InventoryId],
              [i].[Active],
              [i].[ActualChannelNumber],
              [i].[Applicant],
              [i].[BarCode],
              [i].[CheckState],
              [i].[ConfirmedBy],
              [i].[ConfirmedTime],
              [i].[Container],
              [i].[CreateDate],
              [i].[CreateID],
              [i].[Creator],
              [i].[CurrentWipOperationId],
              [i].[DelayRemark],
              [i].[DiversityExplain],
              [i].[DiversityReason],
              [i].[GetSampleBy],
              [i].[GetSampleTime],
              [i].[Hazardlevel],
              [i].[ISDiversity],
              [i].[Inspector],
              [i].[InspectorTime],
              [i].[InventoryStatus],
              [i].[InventoryTestStatus],
              [i].[InventoryType],
              [i].[IsChangCupboard],
              [i].[IsDeleted],
              [i].[IsOrderSparePart],
              [i].[IsReplaced],
              [i].[IsScrapped],
              [i].[LotNo],
              [i].[Modifier],
              [i].[ModifyDate],
              [i].[ModifyID],
              [i].[OrderHeaderId],
              [i].[OrderSampleId],
              [i].[PartnerID],
              [i].[PendingRemark],
              [i].[ProductID],
              [i].[Quantity],
              [i].[RealityManage],
              [i].[ReceiptTime],
              [i].[Recipient],
              [i].[Remark],
              [i].[ReturnReason],
              [i].[SampleACR],
              [i].[SampleOCV],
              [i].[SampleStatus],
              [i].[ScrapRemark],
              [i].[SerialNo],
              [i].[TemporaryManage],
              [i].[TestNo],
              [i].[UPCabinetStatus],
              [i].[WarehouseID],
              [i].[WarehouseLocationID],
              [t1].[WipOperationID],
              [t1].[ATestCode],
              [t1].[ActualChannelNumber] AS [ActualChannelNumber0],
              [t1].[ActualCompletionDate],
              [t1].[ActualDurationSeconds],
              [t1].[ActualLocationId],
              [t1].[ActualStartDate],
              [t1].[AssignTime],
              [t1].[BarCode] AS [BarCode0],
              [t1].[ChannelNumber],
              [t1].[CreateDate] AS [CreateDate0],
              [t1].[CreateID] AS [CreateID0],
              [t1].[Creator] AS [Creator0],
              [t1].[DefaultSamplingTime],
              [t1].[Description],
              [t1].[Description1],
              [t1].[DiversityExplain] AS [DiversityExplain0],
              [t1].[DiversityReason] AS [DiversityReason0],
              [t1].[DownLoadBopBy],
              [t1].[DownLoadBopById],
              [t1].[ExpectedCompletionDate],
              [t1].[ExpectedDurationSeconds],
              [t1].[ExpectedStartDate],
              [t1].[Handler],
              [t1].[HandlerIds],
              [t1].[IsDeleted] AS [IsDeleted0],
              [t1].[IsDiversity] AS [IsDiversity0],
              [t1].[IsInsert],
              [t1].[LocationId],
              [t1].[Modifier] AS [Modifier0],
              [t1].[ModifyDate] AS [ModifyDate0],
              [t1].[ModifyID] AS [ModifyID0],
              [t1].[OperationCode],
              [t1].[OperationId],
              [t1].[OperationName],
              [t1].[OperationStatus],
              [t1].[Operator],
              [t1].[OperatorId],
              [t1].[OprSequenceNo],
              [t1].[OrderHeaderId] AS [OrderHeaderId0],
              [t1].[Position],
              [t1].[ProcessId],
              [t1].[ProcessOperationId],
              [t1].[Remark] AS [Remark0],
              [t1].[Result],
              [t1].[ScheduledCompletionDate],
              [t1].[ScheduledDurationSeconds],
              [t1].[ScheduledStartDate],
              [t1].[TestNo] AS [TestNo0],
              [t1].[WipOperationAssignId],
              [t1].[WipOrderID],
              [t1].[WipOrderID0],
              [t1].[OperationId0] 
             FROM
              [Inventory] AS [i]
              INNER JOIN (
              SELECT
               [w].[WipOperationID],
               [w].[ATestCode],
               [w].[ActualChannelNumber],
               [w].[ActualCompletionDate],
               [w].[ActualDurationSeconds],
               [w].[ActualLocationId],
               [w].[ActualStartDate],
               [w].[AssignTime],
               [w].[BarCode],
               [w].[ChannelNumber],
               [w].[CreateDate],
               [w].[CreateID],
               [w].[Creator],
               [w].[DefaultSamplingTime],
               [w].[Description],
               [w].[Description1],
               [w].[DiversityExplain],
               [w].[DiversityReason],
               [w].[DownLoadBopBy],
               [w].[DownLoadBopById],
               [w].[ExpectedCompletionDate],
               [w].[ExpectedDurationSeconds],
               [w].[ExpectedStartDate],
               [w].[Handler],
               [w].[HandlerIds],
               [w].[IsDeleted],
               [w].[IsDiversity],
               [w].[IsInsert],
               [w].[LocationId],
               [w].[Modifier],
               [w].[ModifyDate],
               [w].[ModifyID],
               [w].[OperationCode],
               [w].[OperationId],
               [w].[OperationName],
               [w].[OperationStatus],
               [w].[Operator],
               [w].[OperatorId],
               [w].[OprSequenceNo],
               [w].[OrderHeaderId],
               [w].[Position],
               [w].[ProcessId],
               [w].[ProcessOperationId],
               [w].[Remark],
               [w].[Result],
               [w].[ScheduledCompletionDate],
               [w].[ScheduledDurationSeconds],
               [w].[ScheduledStartDate],
               [w].[TestNo],
               [w].[WipOperationAssignId],
               [w].[WipOrderID],
               [t].[WipOrderID] AS [WipOrderID0],
               [t].[ACRExtentVmax],
               [t].[ACRExtentVmin],
               [t].[ActualCompletionDate] AS [ActualCompletionDate0],
               [t].[ActualDurationSeconds] AS [ActualDurationSeconds0],
               [t].[ActualStartDate] AS [ActualStartDate0],
               [t].[BarCode] AS [BarCode0],
               [t].[ChannelNumber] AS [ChannelNumber0],
               [t].[CompletedQuantity],
               [t].[ConfirmedBy],
               [t].[ConfirmedDatetime],
               [t].[CreateDate] AS [CreateDate0],
               [t].[CreateID] AS [CreateID0],
               [t].[Creator] AS [Creator0],
               [t].[Duration],
               [t].[ExpectedCompletionDate] AS [ExpectedCompletionDate0],
               [t].[ExpectedDurationSeconds] AS [ExpectedDurationSeconds0],
               [t].[ExpectedStartDate] AS [ExpectedStartDate0],
               [t].[IsDeleted] AS [IsDeleted0],
               [t].[Length],
               [t].[LotNo],
               [t].[Modifier] AS [Modifier0],
               [t].[ModifyDate] AS [ModifyDate0],
               [t].[ModifyID] AS [ModifyID0],
               [t].[OrderDetailId],
               [t].[OrderHeaderId] AS [OrderHeaderId0],
               [t].[OrderLineNo],
               [t].[OrderNo],
               [t].[OrderProcessId],
               [t].[OrderQuantity],
               [t].[Priority],
               [t].[ProcessID] AS [ProcessID0],
               [t].[ProcessName],
               [t].[ProductCode],
               [t].[ProductID],
               [t].[RatedCapacity],
               [t].[RatedPower],
               [t].[RealityManage],
               [t].[ReceivedDate],
               [t].[Remark] AS [Remark0],
               [t].[Remarks],
               [t].[ScheduledCompletionDate] AS [ScheduledCompletionDate0],
               [t].[ScheduledDurationSeconds] AS [ScheduledDurationSeconds0],
               [t].[ScheduledStartDate] AS [ScheduledStartDate0],
               [t].[SerialNo],
               [t].[Spec],
               [t].[SpecialFixture],
               [t].[SpecialTemp],
               [t].[TemporaryManage],
               [t].[TemporaryTime],
               [t].[TestNo] AS [TestNo0],
               [t].[ThicknessVmax],
               [t].[ThicknessVmin],
               [t].[Vmax],
               [t].[Vmin],
               [t].[WarehouseId],
               [t].[WeightVmax],
               [t].[WeightVmin],
               [t].[Width],
               [t].[WipOrderGroupId],
               [t].[WipSequenceNo],
               [t].[WorkOrderStatus],
               [t0].[OperationId] AS [OperationId0],
               [t0].[CreateDate] AS [CreateDate1],
               [t0].[CreateID] AS [CreateID1],
               [t0].[Creator] AS [Creator1],
               [t0].[DefaultSamplingTime] AS [DefaultSamplingTime0],
               [t0].[Description] AS [Description0],
               [t0].[Description1] AS [Description10],
               [t0].[DiscontinueDate],
               [t0].[EffectiveDate],
               [t0].[Enabled],
               [t0].[IsDeleted] AS [IsDeleted1],
               [t0].[Modifier] AS [Modifier1],
               [t0].[ModifyDate] AS [ModifyDate1],
               [t0].[ModifyID] AS [ModifyID1],
               [t0].[OperationCode] AS [OperationCode0],
               [t0].[OperationDay],
               [t0].[OperationName] AS [OperationName0],
               [t0].[OperationType],
               [t0].[Remark] AS [Remark1],
               [t0].[WaterCooler] 
              FROM
               [WipOperation_DoNotUse] AS [w]
               INNER JOIN (
               SELECT
                [w0].[WipOrderID],
                [w0].[ACRExtentVmax],
                [w0].[ACRExtentVmin],
                [w0].[ActualCompletionDate],
                [w0].[ActualDurationSeconds],
                [w0].[ActualStartDate],
                [w0].[BarCode],
                [w0].[ChannelNumber],
                [w0].[CompletedQuantity],
                [w0].[ConfirmedBy],
                [w0].[ConfirmedDatetime],
                [w0].[CreateDate],
                [w0].[CreateID],
                [w0].[Creator],
                [w0].[Duration],
                [w0].[ExpectedCompletionDate],
                [w0].[ExpectedDurationSeconds],
                [w0].[ExpectedStartDate],
                [w0].[IsDeleted],
                [w0].[Length],
                [w0].[LotNo],
                [w0].[Modifier],
                [w0].[ModifyDate],
                [w0].[ModifyID],
                [w0].[OrderDetailId],
                [w0].[OrderHeaderId],
                [w0].[OrderLineNo],
                [w0].[OrderNo],
                [w0].[OrderProcessId],
                [w0].[OrderQuantity],
                [w0].[Priority],
                [w0].[ProcessID],
                [w0].[ProcessName],
                [w0].[ProductCode],
                [w0].[ProductID],
                [w0].[RatedCapacity],
                [w0].[RatedPower],
                [w0].[RealityManage],
                [w0].[ReceivedDate],
                [w0].[Remark],
                [w0].[Remarks],
                [w0].[ScheduledCompletionDate],
                [w0].[ScheduledDurationSeconds],
                [w0].[ScheduledStartDate],
                [w0].[SerialNo],
                [w0].[Spec],
                [w0].[SpecialFixture],
                [w0].[SpecialTemp],
                [w0].[TemporaryManage],
                [w0].[TemporaryTime],
                [w0].[TestNo],
                [w0].[ThicknessVmax],
                [w0].[ThicknessVmin],
                [w0].[Vmax],
                [w0].[Vmin],
                [w0].[WarehouseId],
                [w0].[WeightVmax],
                [w0].[WeightVmin],
                [w0].[Width],
                [w0].[WipOrderGroupId],
                [w0].[WipSequenceNo],
                [w0].[WorkOrderStatus] 
               FROM
                [WipOrder_DoNotUse] AS [w0] 
               WHERE
                [w0].[IsDeleted] = CAST ( 0 AS BIT ) 
               ) AS [t] ON [w].[WipOrderID] = [t].[WipOrderID]
               INNER JOIN (
               SELECT
                [o].[OperationId],
                [o].[CreateDate],
                [o].[CreateID],
                [o].[Creator],
                [o].[DefaultSamplingTime],
                [o].[Description],
                [o].[Description1],
                [o].[DiscontinueDate],
                [o].[EffectiveDate],
                [o].[Enabled],
                [o].[IsDeleted],
                [o].[Modifier],
                [o].[ModifyDate],
                [o].[ModifyID],
                [o].[OperationCode],
                [o].[OperationDay],
                [o].[OperationName],
                [o].[OperationType],
                [o].[Remark],
                [o].[WaterCooler] 
               FROM
                [Operation] AS [o] 
               WHERE
                [o].[IsDeleted] = CAST ( 0 AS BIT ) 
               ) AS [t0] ON [w].[OperationId] = [t0].[OperationId] 
              WHERE
               (
                (
                 (
                  (
                   (
                    ( ( [w].[IsDeleted] = CAST ( 0 AS BIT ) ) AND ( [w].[IsDeleted] = CAST ( 0 AS BIT ) ) ) 
                    AND ( [t].[IsDeleted] = CAST ( 0 AS BIT ) ) 
                   ) 
                   AND ( [t0].[IsDeleted] = CAST ( 0 AS BIT ) ) 
                  ) 
                  AND ( ( [t].[WorkOrderStatus] <> '终止' ) OR [t].[WorkOrderStatus] IS NULL ) 
                 ) 
                 AND ( ( [w].[OperationStatus] = '已指定' ) OR ( [w].[OperationStatus] = '已下发' ) ) 
                ) 
                AND ( [w].[HandlerIds] = 'HD软包--新电芯班组' ) 
               ) 
               AND EXISTS (
               SELECT
                1 
               FROM
                [WipOperation_DoNotUse] AS [w1] 
               WHERE
                ( ( [w1].[IsDeleted] = CAST ( 0 AS BIT ) ) AND ( [t].[WipOrderID] = [w1].[WipOrderID] ) ) 
                AND ( [w1].[Position] = 'JY-HD' ) 
               ) 
              ) AS [t1] ON [i].[CurrentWipOperationId] = [t1].[WipOperationID] 
             WHERE
              ( ( [i].[IsDeleted] = CAST ( 0 AS BIT ) ) AND ( [i].[IsDeleted] = CAST ( 0 AS BIT ) ) ) 
              AND [i].[CurrentWipOperationId] IS NOT NULL 
             ORDER BY
              (SELECT 1) OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY 
             ) AS [t2]
             LEFT JOIN (
             SELECT
              [o0].[OrderHeaderID],
              [o0].[ActualCost],
              [o0].[Applicant],
              [o0].[ApplicantDep],
              [o0].[ApplicantId],
              [o0].[Attachment],
              [o0].[AuditRemark],
              [o0].[CreateDate],
              [o0].[CreateID],
              [o0].[Creator],
              [o0].[DraftStatus],
              [o0].[EstimatedCost],
              [o0].[IsCAEAssessment],
              [o0].[IsCAEProject],
              [o0].[IsDeleted],
              [o0].[IsProductCodeHand],
              [o0].[IsRisk],
              [o0].[IsUrgent],
              [o0].[Modifier],
              [o0].[ModifyDate],
              [o0].[ModifyID],
              [o0].[OAUniqueIdentification],
              [o0].[OrderDate],
              [o0].[OrderHerNo],
              [o0].[OrderStatus],
              [o0].[OrderType],
              [o0].[PackageId],
              [o0].[ProcessClass],
              [o0].[ProcessType],
              [o0].[ProductPN],
              [o0].[ProjectCode],
              [o0].[ProjectName],
              [o0].[ProjectShortCode],
              [o0].[ProjectShortName],
              [o0].[Quantity],
              [o0].[ReceivedQuantity],
              [o0].[Remark],
              [o0].[RequireDescription],
              [o0].[SampleDescribe],
              [o0].[SampleGroup],
              [o0].[SampleSource],
              [o0].[SampleSpec],
              [o0].[SampleSpecName],
              [o0].[SampleStage],
              [o0].[SampleType],
              [o0].[SampleTypeName],
              [o0].[SampleVersion],
              [o0].[SimulationReport],
              [o0].[TaskPeriod],
              [o0].[TaskProject],
              [o0].[TestClass],
              [o0].[TestPurposes],
              [o0].[TestStatus] 
             FROM
              [OrderHeader] AS [o0] 
             WHERE
              [o0].[IsDeleted] = CAST ( 0 AS BIT ) 
             ) AS [t3] ON [t2].[OrderHeaderId] = [t3].[OrderHeaderID]
             LEFT JOIN (
             SELECT
              [w2].[WipOperationID],
              [w2].[ATestCode],
              [w2].[ActualChannelNumber],
              [w2].[ActualCompletionDate],
              [w2].[ActualDurationSeconds],
              [w2].[ActualLocationId],
              [w2].[ActualStartDate],
              [w2].[AssignTime],
              [w2].[BarCode],
              [w2].[ChannelNumber],
              [w2].[CreateDate],
              [w2].[CreateID],
              [w2].[Creator],
              [w2].[DefaultSamplingTime],
              [w2].[Description],
              [w2].[Description1],
              [w2].[DiversityExplain],
              [w2].[DiversityReason],
              [w2].[DownLoadBopBy],
              [w2].[DownLoadBopById],
              [w2].[ExpectedCompletionDate],
              [w2].[ExpectedDurationSeconds],
              [w2].[ExpectedStartDate],
              [w2].[Handler],
              [w2].[HandlerIds],
              [w2].[IsDeleted],
              [w2].[IsDiversity],
              [w2].[IsInsert],
              [w2].[LocationId],
              [w2].[Modifier],
              [w2].[ModifyDate],
              [w2].[ModifyID],
              [w2].[OperationCode],
              [w2].[OperationId],
              [w2].[OperationName],
              [w2].[OperationStatus],
              [w2].[Operator],
              [w2].[OperatorId],
              [w2].[OprSequenceNo],
              [w2].[OrderHeaderId],
              [w2].[Position],
              [w2].[ProcessId],
              [w2].[ProcessOperationId],
              [w2].[Remark],
              [w2].[Result],
              [w2].[ScheduledCompletionDate],
              [w2].[ScheduledDurationSeconds],
              [w2].[ScheduledStartDate],
              [w2].[TestNo],
              [w2].[WipOperationAssignId],
              [w2].[WipOrderID] 
             FROM
              [WipOperation_DoNotUse] AS [w2] 
             WHERE
              [w2].[IsDeleted] = CAST ( 0 AS BIT ) 
             ) AS [t4] ON [t2].[CurrentWipOperationId] = [t4].[WipOperationID]
             INNER JOIN (
             SELECT
              [w3].[WipOrderID],
              [w3].[ACRExtentVmax],
              [w3].[ACRExtentVmin],
              [w3].[ActualCompletionDate],
              [w3].[ActualDurationSeconds],
              [w3].[ActualStartDate],
              [w3].[BarCode],
              [w3].[ChannelNumber],
              [w3].[CompletedQuantity],
              [w3].[ConfirmedBy],
              [w3].[ConfirmedDatetime],
              [w3].[CreateDate],
              [w3].[CreateID],
              [w3].[Creator],
              [w3].[Duration],
              [w3].[ExpectedCompletionDate],
              [w3].[ExpectedDurationSeconds],
              [w3].[ExpectedStartDate],
              [w3].[IsDeleted],
              [w3].[Length],
              [w3].[LotNo],
              [w3].[Modifier],
              [w3].[ModifyDate],
              [w3].[ModifyID],
              [w3].[OrderDetailId],
              [w3].[OrderHeaderId],
              [w3].[OrderLineNo],
              [w3].[OrderNo],
              [w3].[OrderProcessId],
              [w3].[OrderQuantity],
              [w3].[Priority],
              [w3].[ProcessID],
              [w3].[ProcessName],
              [w3].[ProductCode],
              [w3].[ProductID],
              [w3].[RatedCapacity],
              [w3].[RatedPower],
              [w3].[RealityManage],
              [w3].[ReceivedDate],
              [w3].[Remark],
              [w3].[Remarks],
              [w3].[ScheduledCompletionDate],
              [w3].[ScheduledDurationSeconds],
              [w3].[ScheduledStartDate],
              [w3].[SerialNo],
              [w3].[Spec],
              [w3].[SpecialFixture],
              [w3].[SpecialTemp],
              [w3].[TemporaryManage],
              [w3].[TemporaryTime],
              [w3].[TestNo],
              [w3].[ThicknessVmax],
              [w3].[ThicknessVmin],
              [w3].[Vmax],
              [w3].[Vmin],
              [w3].[WarehouseId],
              [w3].[WeightVmax],
              [w3].[WeightVmin],
              [w3].[Width],
              [w3].[WipOrderGroupId],
              [w3].[WipSequenceNo],
              [w3].[WorkOrderStatus] 
             FROM
              [WipOrder_DoNotUse] AS [w3] 
             WHERE
              [w3].[IsDeleted] = CAST ( 0 AS BIT ) 
             ) AS [t5] ON [t2].[WipOrderID] = [t5].[WipOrderID]
             INNER JOIN (
             SELECT
              [w4].[WipOrderGroupId],
              [w4].[ACRExtentVmax],
              [w4].[ACRExtentVmin],
              [w4].[CreateDate],
              [w4].[CreateID],
              [w4].[Creator],
              [w4].[IsDeleted],
              [w4].[Length],
              [w4].[Modifier],
              [w4].[ModifyDate],
              [w4].[ModifyID],
              [w4].[OrderDetailId],
              [w4].[OrderHeaderId],
              [w4].[OrderQuantity],
              [w4].[ProcessID],
              [w4].[ProcessName],
              [w4].[ProductCode],
              [w4].[ProductID],
              [w4].[ProductName],
              [w4].[RatedCapacity],
              [w4].[RatedPower],
              [w4].[Remark],
              [w4].[Spec],
              [w4].[ThicknessVmax],
              [w4].[ThicknessVmin],
              [w4].[Vmax],
              [w4].[Vmin],
              [w4].[WeightVmax],
              [w4].[WeightVmin],
              [w4].[Width] 
             FROM
              [WipOrderGroup] AS [w4] 
             WHERE
              [w4].[IsDeleted] = CAST ( 0 AS BIT ) 
             ) AS [t6] ON [t5].[WipOrderGroupId] = [t6].[WipOrderGroupId]
             INNER JOIN (
             SELECT
              [o1].[OrderDetailId],
              [o1].[Attachment],
              [o1].[BopWorkTime],
              [o1].[CreateDate],
              [o1].[CreateID],
              [o1].[Creator],
              [o1].[DescriptionOne],
              [o1].[FixtureAsk],
              [o1].[IsDeleted],
              [o1].[Modifier],
              [o1].[ModifyDate],
              [o1].[ModifyID],
              [o1].[OrderHeaderID],
              [o1].[OrderLineNo],
              [o1].[OrderProcessId],
              [o1].[OrderStatus],
              [o1].[ProcessClassName],
              [o1].[ProcessID],
              [o1].[ProcessName],
              [o1].[ProcessQuantity],
              [o1].[SampleSpecName],
              [o1].[Sign],
              [o1].[SpecialFixture],
              [o1].[TaskName],
              [o1].[TaskPeriod],
              [o1].[TaskProject],
              [o1].[TaskProjectName],
              [o1].[TaskStandard],
              [o1].[TaskType],
              [o1].[TaskTypeName] 
             FROM
              [OrderDetail] AS [o1] 
             WHERE
              [o1].[IsDeleted] = CAST ( 0 AS BIT ) 
             ) AS [t7] ON [t6].[OrderDetailId] = [t7].[OrderDetailId]
             INNER JOIN (
             SELECT
              [o2].[OrderHeaderID],
              [o2].[ActualCost],
              [o2].[Applicant],
              [o2].[ApplicantDep],
              [o2].[ApplicantId],
              [o2].[Attachment],
              [o2].[AuditRemark],
              [o2].[CreateDate],
              [o2].[CreateID],
              [o2].[Creator],
              [o2].[DraftStatus],
              [o2].[EstimatedCost],
              [o2].[IsCAEAssessment],
              [o2].[IsCAEProject],
              [o2].[IsDeleted],
              [o2].[IsProductCodeHand],
              [o2].[IsRisk],
              [o2].[IsUrgent],
              [o2].[Modifier],
              [o2].[ModifyDate],
              [o2].[ModifyID],
              [o2].[OAUniqueIdentification],
              [o2].[OrderDate],
              [o2].[OrderHerNo],
              [o2].[OrderStatus],
              [o2].[OrderType],
              [o2].[PackageId],
              [o2].[ProcessClass],
              [o2].[ProcessType],
              [o2].[ProductPN],
              [o2].[ProjectCode],
              [o2].[ProjectName],
              [o2].[ProjectShortCode],
              [o2].[ProjectShortName],
              [o2].[Quantity],
              [o2].[ReceivedQuantity],
              [o2].[Remark],
              [o2].[RequireDescription],
              [o2].[SampleDescribe],
              [o2].[SampleGroup],
              [o2].[SampleSource],
              [o2].[SampleSpec],
              [o2].[SampleSpecName],
              [o2].[SampleStage],
              [o2].[SampleType],
              [o2].[SampleTypeName],
              [o2].[SampleVersion],
              [o2].[SimulationReport],
              [o2].[TaskPeriod],
              [o2].[TaskProject],
              [o2].[TestClass],
              [o2].[TestPurposes],
              [o2].[TestStatus] 
             FROM
              [OrderHeader] AS [o2] 
             WHERE
              [o2].[IsDeleted] = CAST ( 0 AS BIT ) 
             ) AS [t8] ON [t7].[OrderHeaderID] = [t8].[OrderHeaderID]
             LEFT JOIN (
             SELECT
              [i0].[InventoryErrorId],
              [i0].[AttachmentInformationIDs],
              [i0].[AttachmentName],
              [i0].[BarCode],
              [i0].[Comment],
              [i0].[ConfirmAt],
              [i0].[ConfirmBy],
              [i0].[ConfirmId],
              [i0].[CreateDate],
              [i0].[CreateID],
              [i0].[Creator],
              [i0].[CurrentState],
              [i0].[ErrorRemark],
              [i0].[ErrorType],
              [i0].[Feedbacktime],
              [i0].[FileName],
              [i0].[InventoryId],
              [i0].[IsDeleted],
              [i0].[IsExplosionProofBox],
              [i0].[Modifier],
              [i0].[ModifyDate],
              [i0].[ModifyID],
              [i0].[TestDuration],
              [i0].[TestTemperature],
              [i0].[UnpackAbnormalRecordDetailID],
              [i0].[WarehouseID],
              [i0].[WarehouseLocationID] 
             FROM
              [InventoryError] AS [i0] 
             WHERE
              [i0].[IsDeleted] = CAST ( 0 AS BIT ) 
             ) AS [t9] ON [t2].[InventoryId] = [t9].[InventoryId] 
            ORDER BY
             [t2].[InventoryId],
             [t2].[WipOperationID],
             [t2].[WipOrderID0],
             [t2].[OperationId0],
             [t5].[WipOrderID],
             [t6].[WipOrderGroupId],
             [t7].[OrderDetailId],
             [t8].[OrderHeaderID],
             [t9].[InventoryErrorId]";

        private const string updateSQL1 = @"
            UPDATE [dbo].[WipOrder_DoNotUse] SET [WorkOrderStatus] = N'待测' WHERE WipOrderId > 241105 AND WipOrderId < 242834;
            WAITFOR DELAY '00:00:05';
            UPDATE [dbo].[WipOperation_DoNotUse] SET [OperationStatus] = N'已排程' WHERE WipOrderId > 241105 AND WipOrderId < 242834;";

        private const string updateSQL2 = @"
            UPDATE [dbo].[WipOrder_DoNotUse] SET [WorkOrderStatus] = N'在测' WHERE WipOrderId > 241105 AND WipOrderId < 242834;
            WAITFOR DELAY '00:00:05';
            UPDATE [dbo].[WipOperation_DoNotUse] SET [OperationStatus] = N'未排程' WHERE WipOrderId > 241105 AND WipOrderId < 242834;";

        private const string insertSQL1 = @"INSERT INTO [dbo].[WipOrder_DoNotUse] (
	                [WipOrderGroupId],
	                [ProductID],
	                [CompletedQuantity],
	                [WorkOrderStatus],
	                [OrderNo],
	                [OrderLineNo],
	                [CreateDate],
	                [CreateID],
	                [Creator],
	                [Modifier],
	                [ModifyDate],
	                [ModifyID],
	                [IsDeleted],
	                [ProcessID],
	                [ProcessName],
	                [OrderQuantity],
	                [Vmax],
	                [Vmin],
	                [Spec],
	                [RatedCapacity],
	                [Duration],
	                [ReceivedDate],
	                [ScheduledStartDate],
	                [ScheduledCompletionDate],
	                [ExpectedStartDate],
	                [ExpectedCompletionDate],
	                [ActualStartDate],
	                [ActualCompletionDate],
	                [ScheduledDurationSeconds],
	                [ExpectedDurationSeconds],
	                [ActualDurationSeconds],
	                [WarehouseId],
	                [BarCode],
	                [TestNo],
	                [LotNo],
	                [TemporaryTime],
	                [TemporaryManage],
	                [Remarks],
	                [SerialNo],
	                [ChannelNumber],
	                [Priority],
	                [WipSequenceNo],
	                [OrderHeaderId],
	                [OrderDetailId],
	                [RealityManage],
	                [ConfirmedBy],
	                [ConfirmedDatetime],
	                [SpecialFixture],
	                [SpecialTemp],
	                [OrderProcessId],
	                [ProductCode],
	                [ACRExtentVmax],
	                [WeightVmax],
	                [ACRExtentVmin],
	                [WeightVmin],
	                [Length],
	                [Width],
	                [ThicknessVmax],
	                [ThicknessVmin],
	                [Remark],
	                [RatedPower] 
                )
                VALUES
	                (
		                224906,
		                NULL,
		                NULL,
		                N'待测',
		                NULL,
		                NULL,
		                '2022-12-20 10:37:44.630',
		                4523,
		                N'刘洋洋',
		                N'刘洋洋',
		                '2022-12-20 11:05:00.943',
		                4523,
		                '0',
		                NULL,
		                NULL,
		                NULL,
		                4.350,
		                2.800,
		                NULL,
		                99.000,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                76,
		                N'mz-lyy-1220-002',
		                N'T22121011M_002',
		                NULL,
		                2,
		                N'报废',
		                NULL,
		                2,
		                NULL,
		                2,
		                1,
		                215214,
		                224438,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                N'A',
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
		                NULL,
                        NULL 
	                );";

        private const string insertSQL2 = @"INSERT INTO [dbo].[WipOperation_DoNotUse] (
	            [WipOrderID],
	            [ProcessId],
	            [ProcessOperationId],
	            [OperationId],
	            [OprSequenceNo],
	            [OperationStatus],
	            [OperationCode],
	            [ScheduledStartDate],
	            [ScheduledCompletionDate],
	            [ExpectedStartDate],
	            [ExpectedCompletionDate],
	            [ActualStartDate],
	            [ActualCompletionDate],
	            [ScheduledDurationSeconds],
	            [ExpectedDurationSeconds],
	            [ActualDurationSeconds],
	            [LocationId],
	            [ActualLocationId],
	            [Result],
	            [Handler],
	            [AssignTime],
	            [Description],
	            [Remark],
	            [HandlerIds],
	            [WipOperationAssignId],
	            [CreateDate],
	            [CreateID],
	            [Creator],
	            [Modifier],
	            [ModifyDate],
	            [ModifyID],
	            [IsDeleted],
	            [Operator],
	            [BarCode],
	            [TestNo],
	            [OrderHeaderId],
	            [ChannelNumber],
	            [ActualChannelNumber],
	            [OperationName],
	            [Position],
	            [DownLoadBopBy],
	            [ATestCode],
	            [DefaultSamplingTime],
	            [Description1],
	            [IsInsert],
	            [IsDiversity],
	            [DiversityReason],
	            [DiversityExplain],
	            [DownLoadBopById],
	            [OperatorId] 
            )
            VALUES
	            (
		            226137,
		            215,
		            1637,
		            218,
		            1,
		            N'已指定',
		            NULL,
		            '2021-08-10 00:00:00.000',
		            '2022-08-10 00:00:00.000',
		            NULL,
		            NULL,
		            NULL,
		            NULL,
		            1.000,
		            NULL,
		            NULL,
		            16933,
		            NULL,
		            NULL,
		            NULL,
		            NULL,
		            NULL,
		            N'导入1',
		            NULL,
		            NULL,
		            NULL,
		            NULL,
		            N'Neal',
		            N'Neal',
		            NULL,
		            NULL,
		            '0',
		            NULL,
		            N'022130720172',
		            N'T21080006C',
		            212962,
		            N'CH2-1',
		            NULL,
		            N'25℃ 100%SOC自放电（首次充电后静置12小时以上） ',
		            N'JY-Lab1',
		            NULL,
		            NULL,
		            NULL,
		            NULL,
		            '0',
		            '0',
		            NULL,
		            NULL,
		            NULL,
		            NULL 
            );";

    }
}
