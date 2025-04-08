import sys
import time
from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC
import logging
import os
from datetime import datetime
from pathlib import Path


class RecordLog(object):
    def __init__(self):
        self.logger = logging.getLogger()
        self.logger.setLevel(logging.DEBUG)
 
        # 2.将log信息输出到log文件中
        # 2.1 先定位看将log文件输出到哪里去
        current_dir = os.path.dirname(os.path.abspath(__file__))
        # print(current_dir)  # D:\MySpace\Python\WebTesting\util
        log_dir = os.path.join(current_dir, '../pylogs')
        # 文件夹不存在创建
        folder = os.path.exists(log_dir)
        # 判断是否存在文件夹如果不存在则创建为文件夹
        if not folder:
            os.makedirs(log_dir)
        # 日志名称构建
        log_file_name = datetime.now().strftime("%Y-%m-%d") + '.log'
        log_file_path = log_dir + '/' + log_file_name
        print(log_file_path)
        # 文件不存在创建
        myfile = Path(log_file_path)
        myfile.touch(exist_ok=True)
 
        # 2.2 好的，将日志写进log文件中
        self.file_handle = logging.FileHandler(log_file_path, 'a', encoding='utf-8')
        formatter = logging.Formatter(
            '%(asctime)s %(filename)s %(funcName)s %(levelno)s: [%(levelname)s] ---> %(message)s')
        self.file_handle.setFormatter(formatter)
        self.logger.addHandler(self.file_handle)
 
    def get_log(self):
        return self.logger
 
    def close_handle(self):
        self.logger.removeHandler(self.file_handle)
        self.file_handle.close()


def do_selenium(url, log):
    try:
        log.info('输入参数：' + str(url))
        option = webdriver.ChromeOptions()
        # #这两行是实现无界面的关键代码
        option.add_argument('--headless')
        # 设置浏览器分辨率
        option.add_argument('window-size=1920x1080')
        # 谷歌文档提到需要加上这个属性来规避bug
        option.add_argument('--disable-gpu')
        # 防止打印一些无用的日志
        option.add_experimental_option("excludeSwitches", ['enable-automation', 'enable-logging'])
        driver = webdriver.Chrome(chrome_options=option)
        # driver = webdriver.Chrome()
        # driver.maximize_window()
        # driver.implicitly_wait(20) #设置隐式等待20秒钟
        # 设置窗口大小
        driver.set_window_size(1440, 1080)
        # 网页加载的最大时间设置为 10s，防止网页因某种原因卡住，导致程序也会卡住
        driver.set_page_load_timeout(10)

        for i in range(3):
            try:
                if i > 0:
                    print('页面加载失败，第%s次重试' % i)
                    log.info('页面加载失败，第%s次重试' % i)
                    time.sleep(2)
                print('请求URL: %s' % url) 
                log.info('请求URL: %s' % url)
                driver.get(url)
                break
            except:
                str1 = "！！！！！！time out when loading page！！！！！！"
                print(str1)
                log.error(str1)
                # 当页面加载时间超过设定时间，通过js来stop，即可执行后续动作
                driver.execute_script("window.stop()")
        # 开始时间
        starttime = datetime.now()

        no_chart = False
        # 等待页面加载
        # for i in range(5) :
        #     print(str(i))
        #     if i == 4:
        #         no_chart = True
        try:
            # 每隔3秒检测一次,15秒超时，直到图表div加载完成
            element = WebDriverWait(driver, timeout=15, poll_frequency=3).until(
                EC.presence_of_element_located((By.CLASS_NAME, "chart"))
            )
            print(element)
            log.info('WebDriver_element: %s' % element)
            pgendtime = datetime.now()
            log.info('页面加载完成,耗时：' + str((pgendtime - starttime).seconds) + '秒')
            print('页面加载完成,耗时：' + str((pgendtime - starttime).seconds) + '秒')
            # break
        except Exception as err:
            #捕捉异常
            str1 = 'Load_Error:' + str(err)
            log.error(str1)
            no_chart = True
            pass
        
        # 页面上没有chart
        if no_chart:
            raise RuntimeError('未检测到chart图表')
        
        print('开始调用js!')
        log.info('开始调用js!')
        resp = driver.execute_async_script("""
            const callback = arguments[arguments.length - 1]
            setTimeout(function () {
                var result1 = window.testFun()
                callback(result1)
            }, 1000)
        """)
        print('获取图表个数:' + str(resp))
        log.debug('获取图表个数:' + str(resp))
        
        # 等待上传完毕结果
        for i in range(50) :
            response = driver.execute_async_script("""
                const callback = arguments[arguments.length - 1]
                setTimeout(function () {
                    var result2 = window.flag
                    callback(result2)
                }, 1000)
            """)
            print('Flag:' + str(response))
            log.debug('Flag:' + str(response))
            if response:
                break
        

    except Exception as err:
        # 捕捉异常
        str1 = 'Error:' + str(err)
        log.error(str1)
    else:
        # 代码运行正常
        str1 = '调用js正常'
        log.info('调用js正常!')
    finally:
        print(str1)
        endtime = datetime.now()
        log.info('程序运行总耗时：' + str((endtime - starttime).seconds) + '秒')
        print('程序运行总耗时：' + str((endtime - starttime).seconds) + '秒')
        # time.sleep(3)
        # a = input()
        driver.close()
        driver.quit()


if __name__=='__main__':
    print('start!')
    try:
        # 加载日志模块
        rl = RecordLog()
        log_info = rl.get_log()
        log_info.info('运行开始!')
        # 获取当前进程ID
        # pid = os.getpid()
        # ppid = os.getppid()
        # log_info.info('当前进程ID:' + str(pid)+ '  ' + str(ppid))
        # print('当前进程ID:' + str(pid) + '  ' + str(ppid))
        # 传入参数
        url = sys.argv[1]
        do_selenium(url, log_info)
        # 这一行代码为本地测试用
        # do_selenium('http://10.202.12.50:8000/#/simpleAnalysis?aid=c91dc7e4-0a7d-4772-9636-6751e260e14a', log_info)
    except Exception as err:
        #捕捉异常
        str1 = 'default:' + str(err)
        log_info.error(str1)
    else:
        # 代码运行正常
        str1 = '运行结束'
    finally:
        log_info.info('运行结束!')
        rl.close_handle()
        # os.popen('taskkill.exe /pid:'+str(pid))
    print(str1)
    os._exit(0)
