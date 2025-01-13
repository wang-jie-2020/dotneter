using demo;

var demo = new ExcelDrawing();
demo.RunDemo();


/*
 *  Excel思路的性能表现，最终确定整体方案结构
 *  （1）读写的能力
 *  （2）至于阅读者能够正常，就不是这里的关心内容了
 *
 *   主要测试要点：
 *      1.传说NPOI直接由文件初始化相比流初始化更快，倾向于认为是无稽之谈
 *      2.NPOI似乎有POI.SXSSFWorkbook（XSSFWorkbook的内存版）处理写操作的内存溢出
 *      3.有建议说以EPPlus替代NPOI
 *      4.思路一还是以硬盘换内存，思路二是直接操作XML
 *      5.基本原则：尽可能降低内存消耗，硬件消耗不是重点，性能上可以忍受一点损失
 */