using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using MathNet.Numerics.Data.Matlab;
using MathNet.Numerics.LinearAlgebra;
using NUnit.Framework;

namespace DoMath;

public class DoMat
{
    private const string tem_bin = @"C:\Users\jie.wang21\Desktop\sd卡解析说明\Bat Temp_2025-04-08_16-14-00.bin";
    private const string tem_mat = @"C:\Users\jie.wang21\Desktop\sd卡解析说明\Bat Temp_2025-04-08_16-14-00.mat";
    private const string tem_all = @"C:\Users\jie.wang21\Desktop\sd卡解析说明\Bat Temp_All.mat";

    void Method1()
    {
        try
        {
            List<MatlabMatrix> ms = MatlabReader.List(tem_mat);
            var finder = ms.Find(o => o.Name == "CellTempData");
            var result = MatlabReader.NonNumeric(finder);
            var structure = result.AsT0;

            Assert.IsTrue(structure.ContainsKey("RackNum"));
            Assert.IsTrue(structure.ContainsKey("data"));

            var rackNumMatrix = structure["RackNum"].AsT2;
            var line = rackNumMatrix.ConcatRows(); // string[]

            var dataMatrix = structure["data"].AsT1;
            var data = dataMatrix.Data;

            /*
             *  T0 MatlabStructure
             *  T1 MatlabCellMatrix
             *  T2 MatlabCharMatrix
             *  T3 MatlabMatrix
             */

            var data00 = data[0, 0].AsT2;
            var data01 = data[0, 1].AsT2;
            var data02 = data[0, 2].AsT2;
            var data03 = data[0, 3].AsT2;
            var data04 = data[0, 4].AsT2;
            var data05 = data[0, 5].AsT2;
            var data06 = data[0, 6].AsT2;

            var data10 = data[1, 0].AsT2;
            var data11 = data[1, 1].AsT3;
            var data12 = data[1, 2].AsT3;
            var data13 = data[1, 3].AsT3;
            var data14 = data[1, 4].AsT3;
            var data15 = data[1, 5].AsT3;
            var data16 = data[1, 6].AsT3;

            var matrix1 = MatlabReader.Unpack<double>(data11);
            var matrix2 = MatlabReader.Unpack<double>(data12);
            var matrix3 = MatlabReader.Unpack<double>(data13);
            var matrix4 = MatlabReader.Unpack<double>(data14);
            var matrix5 = MatlabReader.Unpack<double>(data15);
            var matrix6 = MatlabReader.Unpack<double>(data16);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    void Method2()
    {
        try
        {
            List<NestedObject[]> nested = new List<NestedObject[]>();

            // 以同一个MAT模拟下情况
            for (int count = 0; count < 2; count++)
            {
                List<MatlabMatrix> ms = MatlabReader.List(tem_mat);
                var result = MatlabReader.NonNumeric(ms.Find(o => o.Name == "CellTempData"));
                var structure = result.AsT0;

                var rackNumMatrix = structure["RackNum"].AsT2;
                var dataMatrix = structure["data"].AsT1;

                var data = dataMatrix.Data;

                var rows = data.GetLength(0);
                var cols = data.GetLength(1);

                // 二维数组整理
                for (int i = 0; i < rows; i++)
                {
                    if (i == 0 && count != 0)
                    {
                        continue;
                    }

                    NestedObject[] arr = new NestedObject[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        arr[j] = data[i, j];
                    }

                    nested.Add(arr);
                }
            }

            // 重新写
            if (true)
            {
                var dataMatrix = new MatlabCellMatrix(nested.Count, nested[0].Length);
                for (int i = 0; i < nested.Count; i++)
                {
                    for (int j = 0; j < nested[0].Length; j++)
                    {
                        dataMatrix.Data[i, j] = nested[i][j];
                    }
                }

                var rackNumMatrix = MatlabReader.NonNumeric(MatlabReader.List(tem_mat).Find(o => o.Name == "CellTempData")).AsT0["RackNum"];

                var matlabStructure = new MatlabStructure();
                matlabStructure.Add("RackNum", rackNumMatrix);
                matlabStructure.Add("data", new NestedObject(dataMatrix));
                
                // MemoryStream stream = new MemoryStream();
                // BinaryFormatter formatter = new BinaryFormatter();
                // formatter.Serialize(stream, matlabStructure);



                // var matlabMatrix = new MatlabMatrix(stream.GetBuffer());
                // Parser.ParseNonNumeric(structData.Data);
                
                
                //MatlabWriter.Pack(matlabStructure, "CellTempData");

                //var matrix = new MatlabMatrix("CellTempData", matlabStructure);
                
                // MatlabWriter.Store(tem_all, matrix);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}