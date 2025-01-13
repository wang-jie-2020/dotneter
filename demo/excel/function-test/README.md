# ﻿ Excel-Component

|     组件     | 读性能 | 写性能 |
| :----------: | :----: | :----: |
|     npoi     |   4    |   4    |
|   ep-plus    |   2    |   2    |
| open-xml-sdk |   3    |   3    |
|     linq     |   1    |   1    |

- NPOI
  - 可以直接PASS,直接xml序列化操作,内存爆
  - 在写Excel时提供了SXSSFWorkbook,以硬盘换内存
- EPPlus
  - 综合表现最好
  - 注意前提:
    - (1)SHEET的**数量对性能影响大,读写均是**
    - (2)对CELLS数量的最大支持目前测试结果是4335w,ROWS是39w
- OPEN-SDK
  - 问题如NPOI类似,但注意它的内存控制甚至比EPPlus好
- LINQ
  - 效率最高
  - 必须忍受一些适用性,增加一些复杂性

## 读性能测试

1. 4M - 9.5w cells - 1.8w rows

   |              |      |
   | ------------ | ---- |
   | npoi         | <5s  |
   | ep-plus      | <2s  |
   | open-xml-sdk | <1s  |
   | linq         | -    |

2. 80M - 2400w cells - 21.6w row

   |              |        |
   | ------------ | ------ |
   | npoi         | >20min |
   | ep-plus      | 60s    |
   | open-xml-sdk | 232s   |
   | linq         | 30s    |

3. 500M - (2400w cells - 21.6w row) * 4

   |              |      |
   | ------------ | ---- |
   | npoi         | -    |
   | ep-plus      | 360s |
   | open-xml-sdk | -    |
   | linq         | 130s |

## 写性能测试

1. 5w rows - 100w cells 

   |              |      |
   | ------------ | ---- |
   | npoi         | -    |
   | ep-plus      | 5s   |
   | open-xml-sdk | -    |
   | linq         | 3s   |

2. 50w rows - 1000w cells 

   |              |      |
   | ------------ | ---- |
   | npoi         | -    |
   | ep-plus      | 161s |
   | open-xml-sdk | -    |
   | linq         | 22s  |

补充前提:在LINQ方式中以最简、最极致的方式测试,不是推荐做法,起码也要增加ShardingStrings.xml的反序列化问题


## 关于作图
- NPOI作图略
- EPPlus 作图的可行性已经验证,没什么问题
- 其他    在找一种图表api复制的思路,但似乎没什么头绪