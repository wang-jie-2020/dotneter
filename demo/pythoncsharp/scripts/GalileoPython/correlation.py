import sys
import numpy as np
import pandas as pd
import json


# 计算皮尔逊相关系数
def calc_Pearsons(mainParameters, subParameters):
    try:
        mainParameters_value = json.loads(mainParameters)
        subParameters_value = json.loads(subParameters)

        data = pd.DataFrame({'A': (mainParameters_value), 
                            'B': (subParameters_value)
                            })

        pearsons_value = data.corr()
        pearsons_value_result = pearsons_value.iat[0,1]
        return pearsons_value_result
    except Exception as err:
        return err

# 计算Spearman相关系数
def calc_Spearman(mainParameters, subParameters):
    try:
        mainParameters_value = json.loads(mainParameters)
        subParameters_value = json.loads(subParameters)

        data = pd.DataFrame({'A': (mainParameters_value), 
                            'B': (subParameters_value)
                            })
        
        Spearman_value = data.corr('spearman')
        Spearman_value_result = Spearman_value.iat[0,1]
        return Spearman_value_result
    except Exception as err:
        return err

# 计算Kendall相关系数
def calc_Kendall(mainParameters, subParameters):
    try:
        mainParameters_value = json.loads(mainParameters)
        subParameters_value = json.loads(subParameters)

        data = pd.DataFrame({'A': (mainParameters_value), 
                            'B': (subParameters_value)
                            })

        Kendall_value = data.corr('kendall')
        Kendall_value_result = Kendall_value.iat[0,1]
        return Kendall_value_result
    except Exception as err:
        return err


# 从文本文档中获取参数
def calc_FetchParamters(filePath):
    try:
        with open(filePath, "r") as f:
            data = f.read()
            return data
    except Exception as err:
        return err

# 
def main(mainParamter_filePath, subParamter_filePath):
    mainParamter = calc_FetchParamters(mainParamter_filePath)
    subParamter = calc_FetchParamters(subParamter_filePath)

    pearsons_result = calc_Pearsons(mainParamter, subParamter)
    spearman_result = calc_Spearman(mainParamter, subParamter)
    kendall_result = calc_Kendall(mainParamter, subParamter)

    return{
        "pearsons_result": pearsons_result,
        "spearman_result": spearman_result,
        "kendall_result" :kendall_result
    }


if __name__ == '__main__':
    # print(calc_Pearsons(sys.argv[1],sys.argv[2]))
    # print(calc_Spearman(sys.argv[1],sys.argv[2]))
    # print(calc_Kendall(sys.argv[1],sys.argv[2]))
    print( main(sys.argv[1],sys.argv[2]) ) 
