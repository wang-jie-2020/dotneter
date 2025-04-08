import os
import sys
import numpy as np
import math
import datatable as dt
import matplotlib.pyplot as plt
import pymysql
import json
from scipy import stats
from scipy.stats import kstest
import time
import pandas as pd


# Title: 
# pymysql链接数据库
# Parameters:
# dbset: 数据库链接参数
# sql: 数据库表数据查询SQL
# Return:
# 获取到需要传给python模块的数据
def fetch_sql_list(dbset, sql):
    dbsetting = json.loads(dbset)
    conn = pymysql.connect(host=dbsetting["server"], port=dbsetting["port"], db=dbsetting["database"], user=dbsetting["userid"], password=dbsetting["password"])
    cursor = conn.cursor(pymysql.cursors.DictCursor) 
    cursor.execute(sql)
    data_list = cursor.fetchall()
    conn.close()
    return data_list

# Title:
# 计算一组数据的一系列值
# Parameters:
# data_array: 一组数据
# average: 这组数据的均值
# usl_value: 规格上限
# lsl_value: 规格下限
# Return:
# 计算获得：ppk值、ppu值、ppl值、pp值、sigma_overall（整体标准差）、average（均值）、count（这组数据的数量）
def calc_ppk_ppu_ppl_pp_sigma_overall_average_count(data_array,average,usl_value,lsl_value):
    count=len(data_array)
    if(count == 0 or count == 1):
        return {
            "ppk":0,
            "ppu":0,
            "ppl":0,
            "pp":0,
            "sigma_overall":0,
            "average":average,
            "count":count,

        }
    sigma_overall = np.std(data_array, ddof=1) 

    ppu=0
    ppl=0
    pp=0
    ppk=0
    usl=0
    lsl=0
    if(np.isnan(sigma_overall)==False):
        if(sigma_overall != 0):
            if len(usl_value) > 0:
                usl = float(usl_value)
                ppu = (usl - average) / (3 * sigma_overall)
                # print("65", sigma_overall)
                ppk = ppu
            if len(lsl_value) > 0:
                lsl = float(lsl_value)
                ppl = (average - lsl) / (3 * sigma_overall)
                # print("70", sigma_overall)
                ppk = ppl
            if len(usl_value) > 0 and len(lsl_value) > 0:
                pp = (usl - lsl) / (6 * sigma_overall)
                # print("73", sigma_overall)
                ppk = min(ppu, ppl)
     
    return {
        "ppk":ppk,
        "ppu":ppu,
        "ppl":ppl,
        "pp":pp,
        "sigma_overall":sigma_overall,
        "average":average,
        "count":count
    }

# Title:
# 计算一组数据的PPK值（供给group_dataa数据节点使用）
# Parameters:
# data_array: 一组数据
# average: 这组数据的均值
# usl_value: 规格上限
# lsl_value: 规格下限
# Return:
# 计算获得：ppk值
def calc_ppk_group(data_array,average,usl_value,lsl_value):
    count=len(data_array)
    if(count == 0 or count == 1):
        return None
    sigma_overall = np.std(data_array, ddof=1) 
    ppu=None
    ppl=None
    ppk=None
    usl=None
    lsl=None
    if(np.isnan(sigma_overall)==False):
        if(sigma_overall != 0):
            if len(usl_value) > 0:
                usl = float(usl_value)
                ppu = (usl - average) / (3 * sigma_overall)
                # print("110", sigma_overall)
                ppk = ppu
            if len(lsl_value) > 0:
                lsl = float(lsl_value)
                ppl = (average - lsl) / (3 * sigma_overall)
                # print("116", sigma_overall)
                ppk = ppl
            if len(usl_value) > 0 and len(lsl_value) > 0:
                # 得出ppk
                ppk = min(ppu, ppl)
        
    return ppk

# Title:
# 计算一组数据的组内标准差（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：sigma_within（组内标准差）
def calc_sigma_within(data_array):

    d22 = 1.128 #当只有一组数据时的需要用到d22

    row = 1
    mr_sum = 0
    mr_avg = 0
    rows_count = len(data_array)
    while (row < rows_count):
        a = data_array[row]
        b = data_array[row - 1]
        c = 0
        if( a > b ):
            c = a - b
        else :
            c = b - a
        mr_sum = mr_sum +  c
        row = row + 1
    if rows_count <= 1:
        mr_avg = mr_sum
    else:
        mr_avg = mr_sum / (rows_count - 1)
        # print("151", rows_count)
    sigma_within = mr_avg / d22 
    # print("153", d22)
    return sigma_within

# Title:
# 计算一组数据的总和（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的总和
def calc_sum_group(data_array):
    calc_sum_group_value = sum(data_array)
    return calc_sum_group_value

# Title:
# 计算一组数据的数量（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的数量
def calc_count_group(data_array):
    calc_count_group_value = len(data_array)
    return calc_count_group_value

# Title:
# 计算一组数据的均值（Average）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的均值（Average）
def calc_average_group(data_array):
    calc_average_group_value = np.mean(data_array)
    return calc_average_group_value

# Title:
# 计算一组数据的整体标准差（σ）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的整体标准差（sigma_overall）
def calc_sigma_overall_group(data_array):
    calc_sigma_overall_group_value = np.std(data_array,ddof=1)
    # print("calc_sigma_overall_group_value", calc_sigma_overall_group_value)
    if(np.isnan(calc_sigma_overall_group_value)==False):
        return calc_sigma_overall_group_value
    else:
        return 0

    

# Title:
# 计算一组数据的方差（Variance）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的方差（Variance）
def calc_variance_group(data_array):
    sigmaGroupValue = calc_sigma_overall_group(data_array)
    calc_variance_group_value = math.pow(sigmaGroupValue, 2)
    return calc_variance_group_value

# Title:
# 计算一组数据的变异系数（COV）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的变异系数（COV）
def calc_COV_group(data_array):
    calc_COV_group_value = None
    if(calc_average_group(data_array)==0):
        calc_COV_group_value = ""
    else:
        calc_COV_group_value = calc_sigma_overall_group(data_array) / calc_average_group(data_array)
        # print("225", calc_average_group(data_array))
    return calc_COV_group_value

# Title:
# 计算一组数据的信噪比（SNRate）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的信噪比（SNRate）
def calc_SNRate_group(data_array):
    calc_average_sigma_group_value = None
    if(calc_sigma_overall_group(data_array)==0):
        calc_average_sigma_group_value = ""
    else:
        calc_average_sigma_group_value = 10 * math.log10( math.pow( ( calc_average_group(data_array) / calc_sigma_overall_group(data_array)     ),2 ) )
        # print("240", calc_sigma_overall_group(data_array))
    return calc_average_sigma_group_value

# Title:
# 计算一组数据的最大值（Max）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的最大值（Max）
def calc_Max_group(data_array):
    
    calc_Max_group_value = max(data_array)
    return calc_Max_group_value

# Title:
# 计算一组数据的最小值（Min）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的最小值（Min）
def calc_Min_group(data_array):
    calc_Min_group_value = min(data_array)
    return calc_Min_group_value

# Title:
# 计算一组数据的极值差（Range）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的极值差（Range）
def calc_Range_group(data_array):

    # for data_item in data_array:
    #     # if(type(data_item)==None):
    #     #     print("2333333333")
    #     print(type(data_item))
    
    calc_Range_group_value = calc_Max_group(data_array) - calc_Min_group(data_array)
    # print("calc_Range_group_value", calc_Range_group_value)
    return calc_Range_group_value

# Title:
# 计算四分位数（quantile）的方法(1-4)
# Parameters:
# data_array：一组数据
# n：第几四分位数（第一/第二/第三/第四）
# Return:
# 计算获得：一组数据的四分位数（quantile）
def quantile_exc(data_array, n):   
    data_array.sort()
    position = (len(data_array) + 1)*n/4
    # print("position", position)
    pos_integer = int(math.modf(position)[1])
    # print("pos_integer", pos_integer)
    pos_decimal = position - pos_integer
    # print("data_array_length", len(data_array))
    # print("pos_integerssssssssss", pos_integer)

    if(pos_integer == len(data_array)):
        quartile = data_array[pos_integer - 1]
        # quartile = 0
    else:
        quartile = data_array[pos_integer - 1] + (data_array[pos_integer] - data_array[pos_integer - 1])*pos_decimal
    
    return quartile

# Title:
# 计算一组数据的第一四分位数（Q1）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的第一四分位数（Q1）
def calc_firstquartile_group(data_array):
    # calc_firstquartile_group_value = []
    calc_firstquartile_group_value = quantile_exc(data_array, 1)
    return calc_firstquartile_group_value

# Title:
# 计算一组数据的第三四分位数（Q3）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的第三四分位数（Q3）
def calc_thirdquartile_group(data_array):  
    # calc_thirdquartile_group_value = []  
    calc_thirdquartile_group_value = quantile_exc(data_array, 3)
    # print("calc_thirdquartile_group_value", calc_thirdquartile_group_value)
    return calc_thirdquartile_group_value

# Title:
# 计算一组数据的中位数（Median）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的中位数（Median）
def calc_Medium_group(data_array):
    calc_Medium_group_value = np.median(data_array)
    return calc_Medium_group_value




# Title:
# 计算一组数据的IQR值（IQR）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的IQR值（IQR）
def calc_IQR_group(data_array):
    calc_IQR_group_value = calc_thirdquartile_group(data_array) - calc_firstquartile_group(data_array)
    return calc_IQR_group_value

# Title:
# 计算一组数据的下限（LowerWhisker）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的下限（LowerWhisker）
def calc_LowerWhisker_group(data_array):
    calc_LowerWhisker_group_value = calc_firstquartile_group(data_array) - 1.5 * calc_IQR_group(data_array)
    return calc_LowerWhisker_group_value

# Title:
# 计算一组数据的上限（UpperWhisker值）（供给group_dataa数据节点使用）
# Parameters:
# data_array：一组数据
# Return:
# 计算获得：一组数据的上限（UpperWhisker值）
def calc_UpperWhisker_group(data_array):
    calc_UpperWhisker_within_value = calc_thirdquartile_group(data_array) + 1.5 * calc_IQR_group(data_array)
    return calc_UpperWhisker_within_value

# 计算Pearsons相关系数公式
def calc_Pearsons():    
    data = pd.DataFrame({'A': [1,2,3,4,1,2,3,4,5], 
                         'B': [1,2,3,4,2,3,4,5,6]
                        })

    Pearsons_value = data.corr()
    return Pearsons_value

# 计算Kendall相关系数
def calc_Kendall():
    data = pd.DataFrame({'A': [1,2,3,4,5,6,7,8,8], 
                         'B': [1,2,3,4,5,6,6,7,7]
                        })

    Kendall_value = data.corr('kendall')
    return Kendall_value

# 计算Spearman相关系数
def calc_Spearman():
    data = pd.DataFrame({'A': [6,6,6,5,4,3,2,2,1], 
                         'B': [1,2,3,4,5,6,7,7,7]
                        })
    
    Spearman_value = data.corr('spearman')
    return Spearman_value

# Title:
# 计算一组数据的x轴的一系列坐标值，y轴的一系列坐标值（供给group_dataa数据节点使用）
# Parameters:
# sigma_overall： 整体标准差、average：均值、usl：规格上限、lsl: 规格下限、value_data：一组数据
# Return:
# 计算获得：x_array：一组数据的x轴的一系列坐标值，y_array：y轴的一系列坐标值
def calc_onebar_chart(sigma_overall,average,usl,lsl,value_data):
    # print("#######usl")
    # print(usl)
    # print("#######usl")
    # print("#######lsl")
    # print(lsl)
    # print("#######lsl")
    # if(lsl==""):
    #     print("*******************")

    # 柱状图    
    # x_num = 25
    x_interval = 0.5 * sigma_overall
    if(x_interval==0):
        x_interval = 0.5

    x_start_sigma = average - 6 * sigma_overall
    x_end_sigma = average + 6 * sigma_overall
    # print(x_start_sigma)
    # print(x_end_sigma)

    x_start = None
    x_end = None

    if(usl!="" and lsl!=""):
        if(float(lsl) >= float(x_start_sigma)):
            x_start = float(x_start_sigma)
        else :
            x_start = float(lsl)
        if(float(usl) >= float(x_end_sigma)):
            x_end = float(usl)
        else:
            x_end = float(x_end_sigma)
    if(usl!="" and lsl==""):
        x_start = float(x_start_sigma)
        if(float(usl) >= float(x_end_sigma)):
            x_end = float(usl)
        else:
            x_end = float(x_end_sigma)
    if(usl=="" and lsl!=""):
        x_end = float(x_end_sigma)
        if(float(lsl) >= float(x_start_sigma)):
            x_start = float(x_start_sigma)
        else :
            x_start = float(lsl)
        
    x_start = x_start - x_interval * 2
    x_array = []
    y_array = []
    
    while(x_start <= x_end):
        x_start = x_start + x_interval 
        x_array.append(round(x_start,4))
        y_array.append(0)

    value_data.sort()
    x_len = len(x_array)
    x_startindex = 0
    for v in value_data:
        for x_index in range(x_startindex,x_len):
            x_i = x_array[x_index]
            x_min = x_i - x_interval / 2
            x_max = x_i + x_interval / 2
            if v >= x_min and v < x_max:
                y_array[x_index] = y_array[x_index] + 1
                x_startindex = x_index
                break
    return {
        "x_array":x_array,
        "y_array":y_array
    }

# Title:
# 计算group_date节点中的数据
# Parameters:
# serial_value: 序号、
# data_array：一组数据、
# usl_value：规格上限、
# lsl_value：规格下限、
# isCalculate_value：是否计算（=1 已计算，=0 未计算）
# Return：
# 计算获得：group_date节点中的需要的一系列数据
def calcGroupData(serial_value,data_array,usl_value,lsl_value,isCalculate_value):
    # print("data_array", data_array)

    isCalculate = isCalculate_value
    serialNo = serial_value
    sum = calc_sum_group(data_array)
    number = calc_count_group(data_array)
    average = calc_average_group(data_array)
    standardDeviation = calc_sigma_overall_group(data_array)
    variance = calc_variance_group(data_array)
    cov = calc_COV_group(data_array)
    snRate = calc_SNRate_group(data_array)
    min = calc_Min_group(data_array)
    max = calc_Max_group(data_array)
    range = calc_Range_group(data_array)
    q1 = calc_firstquartile_group(data_array)
    # print("q1", q1)
    median = calc_Medium_group(data_array)
    q3 = calc_thirdquartile_group(data_array)
    # print("q3", q3)
    iqr = calc_IQR_group(data_array)
    lowerWhisker = calc_LowerWhisker_group(data_array)
    upperWhisker = calc_UpperWhisker_group(data_array)
    # print("0")
    ppk = calc_ppk_group(data_array, average, usl_value, lsl_value)
    
    # print("1")
    
    return {
        "isCalculate": isCalculate,
        "serialNo": serialNo,
        "sum": sum,
        "number": number,
        "average": average,
        "standardDeviation": standardDeviation,
        "variance": variance,
        "cov": cov,
        "snRate": snRate,
        "min": min,
        "max": max,
        "range": range,
        "q1": q1,
        "median": median,
        "q3": q3,
        "iqr": iqr,
        "lowerWhisker": lowerWhisker,
        "upperWhisker": upperWhisker,
        "ppk": ppk

    }        

# Title:
# 伽利略系统计算的主函数
# Parameters:
#   dbset: 数据库链接字符串
#   sqlId：分析结果的主键Id(数据库表：GalileoDB_TEST.GALILEO_MODELRECIPEDETAILANALYSISRESULT中的主键Id)
#   usl_value: 规格上限
#   lsl_value: 规格下限
#   table_value_key: CPK计算值列名，（数据库表：GalileoDB_TEST.GALILEO_DATAMODELPARAMETER中的DataColumns）一般就是：VALUE、TESTVALUE、PVALUE
#   group_mincount: 组内样本数下限
# Return:
# 计算获得：
# 均值（average）
# ppk_sigma（sigma_overall）
# pp
# ppl
# ppu
# ppk
# cpk_sigma（sigma_within）
# cp
# cpl
# cpu
# cpk
# cpm
# max（usl_value）
# min（lsl_value）
# count（多少条数据）
# x_array（坐标轴X轴数据）
# y_array（坐标轴Y轴数据）
# minnumber（组内样本数下限）
# sn_rate
# sw_test
# ks_test
# ppk_linechart_data（总图表的数据）
# last_group_linechart_data（最后一组图表数据）
# group_data_all数据
def calc(dbset,sqlId,usl_value,lsl_value,table_value_key,table_group_key,group_mincount):

    usl=""
    lsl=""
    if len(usl_value) > 0:
        usl = float(usl_value)
    if len(lsl_value) > 0:
        lsl = float(lsl_value)
    
    dbset = dbset.replace("'", "\"")
    sqldatatable_sql = "select * from GALILEO_MODELRECIPEDETAILANALYSISRESULT where id = '" + sqlId + "'" 
    sqldatatable = fetch_sql_list(dbset,sqldatatable_sql)    
    sqls = ""
    for s in sqldatatable:
        sqls=s["MappingSQL"]
    # print("sqls=",sqls)
    sqlmodel = json.loads(sqls) 
    sql = sqlmodel["PythonCalcSQL"] #获取到分组的数据（SQL语句）
    # print(sql)
    data_list = fetch_sql_list(dbset,sql) #获取到分析结果的所有数据
    # print(data_list)
    python_group_data = json.loads(sqlmodel["PythonGroupData"]) #获取到分组的条件
    
    value_data=[] # 声明所有数据
    group_data=[] # 声明所有分组数据
    x_array_all=[] # 声明x轴数组数据
    y_array_all=[] # 声明y轴数组数据
    group_list=[] # 声明要被分组的数据
    avg_y_array=[] # 声明y轴数组数据的均值
    count_y_array=[] # 声明y轴数组数据的个数
    group_data_array=[] # 声明group_data_array节点的数组数据
    lastgroup_value_data=[] # 声明最后一组所有数据
    lastgroup_within_data=[] # 声明最后一组所有分析数据

    OUTPUTLOTNO_data=[]
    TOTALPROCESSED_data=[]
    
    # print("python_group_data",python_group_data)

    for g_data in python_group_data:
        x_array_all.append(g_data["GroupStartTime"])
        y_array_all.append(0)
        avg_y_array.append(0)
        count_y_array.append(0)
        group_list.append([])

        # print("OUTPUTLOTNO", g_data["OUTPUTLOTNO"])
        # print("TOTALPROCESSED", g_data["TOTALPROCESSED"])

        # if(g_data["OUTPUTLOTNO"]!=""):
        #     OUTPUTLOTNO_data.append(g_data["OUTPUTLOTNO"]) # 极卷号
        # if(g_data["TOTALPROCESSED"]!=""):
        #     TOTALPROCESSED_data.append(g_data["TOTALPROCESSED"]) # 极卷号的米数

        if("OUTPUTLOTNO" in g_data):
            # print("OUTPUTLOTNO")
            OUTPUTLOTNO_data.append(g_data["OUTPUTLOTNO"]) # 极卷号
        if("TOTALPROCESSED" in g_data):
            # print("TOTALPROCESSED")
            TOTALPROCESSED_data.append(g_data["TOTALPROCESSED"]) # 极卷号的米数

        group_data_array.append([])

    if len(group_list) == 0:
        group_list.append([])

    if len(group_data_array) == 0:
        group_data_array.append([])

    # print("data_list")
    # print(data_list)
    for data_list_item in data_list:
        # print("wwww", type(data_list_item[table_value_key]))
        if(data_list_item[table_value_key] == None):
            data_list_item[table_value_key] = 0
        value_data.append(data_list_item[table_value_key])
        if(table_group_key == '1'):
            group_data.append(1)
        else:
            group_data.append(data_list_item[table_group_key])

        
    value_data_len = len(value_data)
    if value_data_len == 0 :
        returndata = {
            "average":"",
            "ppk_sigma":"",
            "pp":"",
            "ppl":"",
            "ppu":"",
            "ppk":"",
            "cpk_sigma":"",
            "cp":"",
            "cpl":"",
            "cpu":"",
            "cpk":"",
            "cpm":"",
            "max":usl_value,
            "min":lsl_value,
            "count":"",
            "x_array":[],
            "y_array":[],
            "minnumber":group_mincount,
            "sn_rate":"",
            "sw_test":"",
            "ks_test":"",
            "ppk_linechart_data":{
                "x_array":[],
                "y_array":[]
            }
        }
        return json.dumps(returndata)
    
    if value_data_len == 1 :
        returndata = {
            "average":value_data[0], # 因为只有一组数据
            "ppk_sigma":"",
            "pp":"",
            "ppl":"",
            "ppu":"",
            "ppk":"",
            "cpk_sigma":"",
            "cp":"",
            "cpl":"",
            "cpu":"",
            "cpk":"",
            "cpm":"",
            "max":usl_value,
            "min":lsl_value,
            "count":1,
            "x_array":[],
            "y_array":[],
            "minnumber":group_mincount,
            "sn_rate":"",
            "sw_test":"",
            "ks_test":"",
            "ppk_linechart_data":{
                "x_array":[],
                "y_array":[]
            }
        }
        return json.dumps(returndata);
    df_data = dt.Frame({table_value_key:value_data, table_group_key:group_data})
    # print(df_data)
    
    c4_array = [1,1,0.7979,0.8862,0.9213,0.9400,0.9515,0.9594,0.9650,0.9693,0.9727,0.9754,0.9776,0.9794,0.9810,0.9823,0.9835,0.9845,0.9854,0.9862,0.9869,0.9876,0.9882,0.9887,0.9892,0.9896]
    
    # 平均值
    average = df_data[:,table_value_key].mean()[0,table_value_key]

    # Ppk数据标准差
    ppk_data = calc_ppk_ppu_ppl_pp_sigma_overall_average_count(value_data,average,usl_value,lsl_value)
    sigma_overall = ppk_data["sigma_overall"]
    ppu = ppk_data["ppu"]
    ppl = ppk_data["ppl"]
    pp = ppk_data["pp"]
    ppk = ppk_data["ppk"] 

    rows_count = df_data[:,dt.count()][0,'count'] #所有的数据行数
    row = 0 #声明行数
    groupcount = 0 #声明组数

    # 判断组数
    if(table_group_key == '1'):
        groupcount = 1
    else :
        gindex = 0
        # 循环所有的行，按组分成数组
        while (row < rows_count):
            group = df_data[row,table_group_key]
            gindex = group - 1
            group_list[gindex].append(df_data[row,table_value_key])
            group_data_array[gindex].append(df_data[row,table_value_key])

            row = row + 1
        groupcount = len(group_list) #所有组数


        # print("group_list")
        # print(group_list)
        
    sigma_within = 0
    cpu = 0
    cpl = 0
    cp = 0
    cpk = 0

    group_dataa_array_all = {
        "x_array":[],
        "group_data":[]
    }

    global group_dataa_array
    group_dataa_array = []
    
    # print(value_data)
    # 参考文档 https://support.minitab.com/zh-cn/minitab/20/help-and-how-to/quality-and-process-improvement/control-charts/how-to/variables-charts-for-subgroups/xbar-r-chart/methods-and-formulas/estimating-sigma/#rbar-method
    # 一组时的组内标准差
    if(groupcount == 1):
        sigma_within = calc_sigma_within(value_data)
        # print(sigma_within)    
    # 多组时的组内标准差      
    else:
        all_group_square_sum = 0 # 所有组的平方和
        # 所有组的数据数量总和 减  组的总数
        all_zi = 0 # 声明一组内的数量
        grindex = -1
        for groupdetail in group_list:
            # print(groupdetail)
            groupdetail_count = len(groupdetail) #组内样本数
            do_calc = 0 #是否计算(注：组内样本数下限>=0以及组内样本数下限<组内样本数的时候，才能计算，0表示不能计算，1表示可以计算)
            grindex = grindex + 1
            if groupdetail_count > 0:              
                if (int(group_mincount) >= 0 and int(group_mincount) <= int(groupdetail_count)):
                    do_calc = 1
            if groupdetail_count == 0:
                    do_calc = 0

            if do_calc == 1:
                # 组内均值
                group_avg = np.mean(groupdetail)
                # 组内所有数据与均值的差的平方和
                group_square_sum = 0
                for groupvalue in groupdetail:
                    group_square_sum = group_square_sum + (groupvalue - group_avg) ** 2

                    # print("groupvalue")
                    # print(json.dumps(groupvalue))

                all_group_square_sum = all_group_square_sum + group_square_sum
                all_zi = all_zi + groupdetail_count -1

                isCalculate = 1

                # print("grindex",grindex)
                # print("groupdetail", groupdetail) 
                group_dataa = calcGroupData(grindex, groupdetail, usl_value, lsl_value, isCalculate) 
                # print("group_dataa",group_dataa)
                group_dataa_array.append(group_dataa) 
                
                #组内所有参数计算
                group_within_analysis_data = calc_ppk_ppu_ppl_pp_sigma_overall_average_count(groupdetail,group_avg,usl_value,lsl_value)                 
                y_array_all[grindex] = group_within_analysis_data["ppk"]

                avg_y_array[grindex] = group_within_analysis_data["average"]

                count_y_array[grindex] = group_within_analysis_data["count"]

                
               
                # 为了拿到最后一组数据以及计算分析的数据
                if grindex + 1 == len(group_list):
                    lastgroup_value_data = groupdetail
                    lastgroup_within_data = group_within_analysis_data
        
        # print(groupdetail_count)
        # print(group_mincount)

        # 使用c4时，需要使用 c4_array ，而index取上面的 all_zi ，且应在1至24之间
        c4_index = all_zi
        if(c4_index > 24):
            c4_index = 24
        if(c4_index < 2):
            c4_index = 1
        if(all_zi!=0):
            sigma_within = math.sqrt(all_group_square_sum / all_zi) / c4_array[c4_index + 1]
            # print("812_all_zi", all_zi)
            # print("812_c4_array", c4_array[c4_index + 1])
    if(sigma_within == 0):      
        sigma_within = sigma_overall
        # print(sigma_within)
        
    if(sigma_within != 0):
        if len(usl_value) > 0:
            cpu = (usl - average) / (3 * sigma_within)
            # print("821", sigma_within)
            cpk = cpu
        if len(lsl_value) > 0:
            cpl = (average - lsl) / (3 * sigma_within)
            # print("825", sigma_within)
            cpk = cpl
        if len(usl_value) > 0 and len(lsl_value) > 0:
            cp = (usl - lsl) / (6 * sigma_within)
            # print("829", sigma_within)
            # 得出ppk
            cpk = min(cpu, cpl)
    

    cpm = 0
    if len(usl_value) > 0 and len(lsl_value) > 0:
        cpm = (usl - lsl) / ( 6 * math.sqrt( (average - ((usl + lsl) / 2)) ** 2 + sigma_overall ** 2 ) )
        # print("837", 6 * math.sqrt( (average - ((usl + lsl) / 2)) ** 2 + sigma_overall ** 2 ))

    
    snrate_value = ""
    swtest_value = ""
    kstest_value = ""

    # if snrate_calc == 1 and sigma_overall != 0:
    #     snrate_value = round(10 * math.log10( ( average / sigma_overall ) ** 2 ),4)
    if sigma_overall != 0:
        snrate_value = round(10 * math.log10( ( average / sigma_overall ) ** 2 ),4)
        # print("847", sigma_overall)

    # if swtest_calc == 1:
    #     vlen=len(value_data)
    #     if(vlen>=3 and vlen<=5000):
    #         swresult = stats.shapiro(value_data)
    #         swtest_value = round(swresult.pvalue,4)
    
    # if kstest_calc == 1:
    #     ksresult = kstest(value_data,"norm")
    #     kstest_value = round(ksresult.pvalue,4)

    barchart_analysis_data = calc_onebar_chart(sigma_overall,average,usl,lsl,value_data)
    x_array = barchart_analysis_data["x_array"]
    y_array = barchart_analysis_data["y_array"]
    

    last_group_x_array = []
    last_group_y_array = []
    last_group_cpk_data={
        "cpk_sigma":"",
        "cp":"",
        "cpl":"",
        "cpu":"",
        "cpk":""
    }

    
    # print(last_group_value_data)

    if len(lastgroup_value_data) > 0  :
        last_group_barchart_data = calc_onebar_chart(lastgroup_within_data["sigma_overall"],lastgroup_within_data["average"],usl_value,lsl_value,lastgroup_value_data)
        last_group_x_array = last_group_barchart_data["x_array"]
        last_group_y_array = last_group_barchart_data["y_array"]
        last_group_sigma_within = calc_sigma_within(lastgroup_value_data) 

        last_group_cp = 0
        last_group_cpl = 0
        last_group_cpu = 0
        last_group_cpk = 0
        if(last_group_sigma_within == 0):
            last_group_sigma_within = lastgroup_within_data["sigma_overall"]
        if(last_group_sigma_within != 0):
            if len(usl_value) > 0:
                last_group_cpu = (usl - lastgroup_within_data["average"]) / (3 * last_group_sigma_within)
                # print("893", last_group_sigma_within)
                last_group_cpk = last_group_cpu
            if len(lsl_value) > 0:
                last_group_cpl = (lastgroup_within_data["average"] - lsl) / (3 * last_group_sigma_within)
                # print("897", last_group_sigma_within)
                last_group_cpk = last_group_cpl
            if len(usl_value) > 0 and len(lsl_value) > 0:
                last_group_cp = (usl - lsl) / (6 * last_group_sigma_within)
                # print("901", last_group_sigma_within)
                last_group_cpk = min(last_group_cpu, last_group_cpl) 
        last_group_cpm = 0
        if len(usl_value) > 0 and len(lsl_value) > 0:
            last_group_cpm = (usl - lsl) / ( 6 * math.sqrt( (lastgroup_within_data["average"] - ((usl + lsl) / 2)) ** 2 + lastgroup_within_data["sigma_overall"] ** 2 ) )
            # print("906", math.sqrt( (lastgroup_within_data["average"] - ((usl + lsl) / 2)) ** 2 + lastgroup_within_data["sigma_overall"] ** 2 ))
        last_group_cpk_data = {
            "cpk_sigma":last_group_sigma_within,
            "cp":last_group_cp,
            "cpl":last_group_cpl,
            "cpu":last_group_cpu,
            "cpk":last_group_cpk,
            "cpm":last_group_cpm
        } 
        # print(last_group_cpk_data)


    # ppk_linechart_data = {
    #     "x_array":[],
    #     "y_array":[],
    #     "avg_array": [],
    #     "count_array": [],
    #     "OUTPUTLOTNO_array": [],
    #     "TOTALPROCESSED_array": []
    # }

    

    ppk_linechart_data = {
        "x_array":x_array_all,
        "y_array":y_array_all,
        "avg_array": avg_y_array,
        "count_array": count_y_array,
        "OUTPUTLOTNO_array": OUTPUTLOTNO_data,
        "TOTALPROCESSED_array": TOTALPROCESSED_data
    }

    # print("ppk_linechart_data", ppk_linechart_data)

    last_group_linechart_data= {
        "average":None,
        "ppk_sigma":None,
        "pp":None,
        "ppl":None,
        "ppu":None,
        "ppk":None,
        "cpk_sigma":None,
        "cp":None,
        "cpl":None,
        "cpu":None,
        "cpk":None,
        "cpm":None,
        "max":None,
        "min":None,
        "count":None,
        "x_array":[],
        "y_array":[]
    }
    if len(last_group_x_array)>0 and len(last_group_y_array) > 0:
        last_group_linechart_data= {
            "average":lastgroup_within_data["average"],
            "ppk_sigma":lastgroup_within_data["sigma_overall"],
            "pp":lastgroup_within_data["pp"],
            "ppl":lastgroup_within_data["ppl"],
            "ppu":lastgroup_within_data["ppu"],
            "ppk":lastgroup_within_data["ppk"],
            # "cpk_sigma":last_group_cpk_data["cpk_sigma"],
            # "cp":last_group_cpk_data["cp"],
            # "cpl":last_group_cpk_data["cpl"],
            # "cpu":last_group_cpk_data["cpu"],
            # "cpk":last_group_cpk_data["cpk"],
            # "cpm":last_group_cpk_data["cpm"],
            "max":usl_value,
            "min":lsl_value,
            "count":len(lastgroup_value_data),
            "x_array":last_group_x_array,
            "y_array":last_group_y_array
        }

    group_dataa_array_all["x_array"] = x_array_all
    group_dataa_array_all["group_data"] = group_dataa_array

    # print("group_dataa_array_all")
    # print(json.dumps(group_dataa_array_all))

    returndata = {
        "average":average,
        "ppk_sigma":sigma_overall,
        "pp":pp,
        "ppl":ppl,
        "ppu":ppu,
        "ppk":ppk,
        "cpk_sigma":sigma_within,
        "cp":cp,
        "cpl":cpl,
        "cpu":cpu,
        "cpk":cpk,
        "cpm":cpm,
        "max":usl_value,
        "min":lsl_value,
        "count":rows_count,
        "x_array":x_array,
        "y_array":y_array,
        "minnumber":group_mincount,
        "sn_rate":snrate_value,
        "sw_test":swtest_value,
        "ks_test":kstest_value,
        "ppk_linechart_data":ppk_linechart_data,
        "last_group_linechart_data":last_group_linechart_data,
        "group_data_all": group_dataa_array_all
    }

    # print(json.dumps(returndata))
    return json.dumps(returndata);

     
if __name__=='__main__':
    try:
        #代码行
        # dbset = sys.argv[1]
        # sql = sys.argv[2]
        # usl = sys.argv[3]
        # lsl = sys.argv[4]
        # table_value_key = sys.argv[5]
        # table_group_key = sys.argv[6]
        # mincount = float(sys.argv[7])
       
        # result = calc(dbset,sql,usl,lsl,table_value_key,table_group_key,mincount) # 正式的主函数
        # result = calc("{'server':'10.202.8.61','port':3306,'userid':'GalileoDB_TEST','password':'Wdv$2021','database':'GalileoDB_TEST'}","d48b6924-2585-46e0-a95a-c7275ebb5db5","0.2","" ,"VALUE" ,"GroupColumn","100" )
        result = calc_Kendall()
        # result = calc_Spearman()
        # result = calc_Pearsons()
    except Exception as err:
        #捕捉异常
        str1 = 'default:' + str(err)
    else:
        # 代码运行正常
        str1 = result
    print(str1)

        
