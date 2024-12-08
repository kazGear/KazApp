using CSLib.Lib;

/*
 print util
 */
void Print(object o) => Console.WriteLine(o);
void PrintAll<T>(IEnumerable<T> os)
{
    foreach (T o in os) Console.WriteLine(o!.ToString());
}

/************************************************************
 * 
 * 汎用テストエリア
 * 
 ************************************************************/

bool doTest = true; // テスト稼働可否
if (!doTest) return;

// testtStart=======================
Print(">>> test info.");
UTimeMeasure.Start();
// =================================

// 復号
string result = UAes.AesDecrypt("oBE87gjbotORkFKy+qEjbQ==");
Console.WriteLine(result);
Console.WriteLine(result.Length);

string result2 = UAes.AesDecrypt("44oSRDDfZKdXGPimoJgZsA==");
Console.WriteLine(result2);
Console.WriteLine(result2.Length);










// ============================
UTimeMeasure.Stop();
Print(">>> test end...");
// testEnd======================

