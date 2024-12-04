using CSLib.Lib;
using KazApi.Common._Log;
using KazApi.Controller.Service;
using KazApi.Domain._Const;
using KazApi.Domain._Factory;
using KazApi.Domain._GameSystem;
using KazApi.Domain._Monster;
using KazApi.Domain._Monster._Skill;
using KazApi.Domain._Monster._State;
using KazApi.Domain.DTO;
using KazApi.Repository;
using KazApi.Repository.sql;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Net;
using System.Runtime.InteropServices;


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

