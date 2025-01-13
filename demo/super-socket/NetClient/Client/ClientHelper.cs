//namespace NetClient.Client
//{
//    public class ClientHelper
//    {
//        public static Dictionary<string, NetSocket> dNetSocket = new Dictionary<string, NetSocket>();

//        public static event DeviceInformation OnDeviceInfo;

//        public static event StartReturnResultInfo OnStartReturnResult;

//        public static event StopReturnResultInfo OnStopReturnResult;

//        public static event PauseReturnResultInfo OnPauseReturnResult;

//        public static event ContinueReturnResultInfo OnContinueReturnResult;

//        public static event JumpReturnResultInfo OnJumpReturnResult;

//        public static event ClearReturnResultInfo OnClearReturnResult;

//        public static event MdCmdReturnResultInfo OnMdCmdReturnResult;

//        public static event AuxRealDataInfo OnAuxRealData;

//        public static event RealDataModelInfo OnRealData;

//        public static event DCIRModelInfo OnDCIRData;

//        public static event VoltModelInfo OnVoltData;

//        public static event StepDataInfo OnStepInfo;

//        public static event BmsRealDataInfo OnBmsRealData;

//        public static void ReadData(string sHost, string revString)
//        {
//            if (revString.Trim() == "")
//                return;
//            try
//            {
//                string[] strArray = revString.Split('|');
//                for (int index1 = 0; index1 < strArray.Length; ++index1)
//                {
//                    if (!(strArray[index1].Trim() == ""))
//                    {
//                        CmdModel cmdModel = JsonHelper.DeserializeJsonToObject<CmdModel>(strArray[index1]);
//                        switch (cmdModel.Cmd)
//                        {
//                            case "0300":
//                                OnDeviceInfo(JsonHelper.DeserializeJsonToObject<List<DeviceInfoModel>>(cmdModel.Jstring));
//                                LogHelper.WriteLog(string.Format("获取设备信息：{0}", (object)cmdModel.Jstring), p_ltType: LogType.Info);
//                                break;
//                            case "0301":
//                                OnStartReturnResult(JsonHelper.DeserializeJsonToObject<ReturnResult>(cmdModel.Jstring));
//                                LogHelper.WriteLog(string.Format("启动返回值：{0}", (object)cmdModel.Jstring), p_ltType: LogType.Info);
//                                break;
//                            case "0302":
//                                OnStopReturnResult(JsonHelper.DeserializeJsonToObject<ReturnResult>(cmdModel.Jstring));
//                                LogHelper.WriteLog(string.Format("停止返回值：{0}", (object)cmdModel.Jstring), p_ltType: LogType.Info);
//                                break;
//                            case "0303":
//                                OnPauseReturnResult(JsonHelper.DeserializeJsonToObject<ReturnResult>(cmdModel.Jstring));
//                                LogHelper.WriteLog(string.Format("暂停返回值：{0}", (object)cmdModel.Jstring), p_ltType: LogType.Info);
//                                break;
//                            case "0304":
//                                OnContinueReturnResult(JsonHelper.DeserializeJsonToObject<ReturnResult>(cmdModel.Jstring));
//                                LogHelper.WriteLog(string.Format("继续返回值：{0}", (object)cmdModel.Jstring), p_ltType: LogType.Info);
//                                break;
//                            case "0305":
//                                OnJumpReturnResult(JsonHelper.DeserializeJsonToObject<ReturnResult>(cmdModel.Jstring));
//                                LogHelper.WriteLog(string.Format("跳转返回值：{0}", (object)cmdModel.Jstring), p_ltType: LogType.Info);
//                                break;
//                            case "0306":
//                                OnClearReturnResult(JsonHelper.DeserializeJsonToObject<ReturnResult>(cmdModel.Jstring));
//                                LogHelper.WriteLog(string.Format("清除故障返回值：{0}", (object)cmdModel.Jstring), p_ltType: LogType.Info);
//                                break;
//                            case "0101":
//                                RealTimeDataModel realTimeDataModel1 = JsonHelper.DeserializeJsonToObject<RealTimeDataModel>(cmdModel.Jstring);
//                                using (List<List<object>>.Enumerator enumerator = realTimeDataModel1.data.GetEnumerator())
//                                {
//                                    while (enumerator.MoveNext())
//                                    {
//                                        List<object> current = enumerator.Current;
//                                        RealDataModel realDataModel = new RealDataModel()
//                                        {
//                                            Adrr = realTimeDataModel1.adrr,
//                                            masterIp = sHost,
//                                            Channel = Convert.ToString(current[0]),
//                                            Status = Convert.ToInt32(current[1]),
//                                            Starttime = Convert.ToDouble(current[2]),
//                                            Stepruntime = Convert.ToDouble(current[3]),
//                                            Runtime = Convert.ToDouble(current[4]),
//                                            Cycleid = Convert.ToInt32(current[5]),
//                                            Stepid = Convert.ToInt32(current[6]),
//                                            Stepnum = Convert.ToInt32(current[7]),
//                                            Stepname = Convert.ToInt32(current[8]),
//                                            Voltage = Convert.ToDouble(current[9]),
//                                            Current = Convert.ToDouble(current[10]),
//                                            Temperature = Convert.ToDouble(current[11]),
//                                            Capacity = Convert.ToDouble(current[12]),
//                                            Energy = Convert.ToDouble(current[13]),
//                                            Resistance = Convert.ToDouble(current[14]),
//                                            CycChagerCapacity = Convert.ToDouble(current[15]),
//                                            CycChagerEnergy = Convert.ToDouble(current[16]),
//                                            CycDischagerCapacity = Convert.ToDouble(current[17]),
//                                            CycDischagerEnergy = Convert.ToDouble(current[18]),
//                                            Dcalarm = Convert.ToInt64(current[19]),
//                                            Acalarm = Convert.ToInt64(current[20])
//                                        };
//                                        OnRealData(realDataModel);
//                                    }
//                                    break;
//                                }
//                            case "0102":
//                                RealTimeDataModel realTimeDataModel2 = JsonHelper.DeserializeJsonToObject<RealTimeDataModel>(cmdModel.Jstring);
//                                using (List<List<object>>.Enumerator enumerator = realTimeDataModel2.data.GetEnumerator())
//                                {
//                                    while (enumerator.MoveNext())
//                                    {
//                                        List<object> current = enumerator.Current;
//                                        AuxRealDataModel auxRealData = new AuxRealDataModel()
//                                        {
//                                            Adrr = realTimeDataModel2.adrr,
//                                            Chid = Convert.ToString(current[0]),
//                                            VNum = Convert.ToInt32(current[1]),
//                                            AuxV = JsonHelper.DeserializeJsonToObject<List<double>>(current[2].ToString()),
//                                            TNum = Convert.ToInt32(current[3]),
//                                            AuxT = JsonHelper.DeserializeJsonToObject<List<double>>(current[4].ToString())
//                                        };
//                                        OnAuxRealData(auxRealData);
//                                    }
//                                    break;
//                                }
//                            case "0103":
//                                RealTimeDataModel realTimeDataModel3 = JsonHelper.DeserializeJsonToObject<RealTimeDataModel>(cmdModel.Jstring);
//                                foreach (List<object> objectList in realTimeDataModel3.data)
//                                {
//                                    BmsRealDataModel bmsDataModel = new BmsRealDataModel()
//                                    {
//                                        adrr = realTimeDataModel3.adrr,
//                                        channel = objectList[0].ToString()
//                                    };
//                                    bmsDataModel.bmsDatas = new List<BmsData>();
//                                    List<List<string>> stringListList = JsonHelper.DeserializeJsonToObject<List<List<string>>>(objectList[1].ToString());
//                                    for (int index2 = 0; index2 < stringListList.Count; ++index2)
//                                        bmsDataModel.bmsDatas.Add(new BmsData()
//                                        {
//                                            ParaName = stringListList[index2][0],
//                                            ParaValue = stringListList[index2][1]
//                                        });
//                                    OnBmsRealData(bmsDataModel);
//                                }
//                                OnBmsRealData(JsonHelper.DeserializeJsonToObject<BmsRealDataModel>(cmdModel.Jstring));
//                                break;
//                            case "0401":
//                                OnDCIRData(JsonHelper.DeserializeJsonToObject<DCIRModel>(cmdModel.Jstring));
//                                break;
//                            case "0402":
//                                OnStepInfo(JsonHelper.DeserializeJsonToObject<StepInfoMessage>(cmdModel.Jstring));
//                                break;
//                            case "0403":
//                                OnMdCmdReturnResult(JsonHelper.DeserializeJsonToObject<ReturnResult>(cmdModel.Jstring));
//                                LogHelper.WriteLog(string.Format("清除故障返回值：{0}", (object)cmdModel.Jstring), p_ltType: LogType.Info);
//                                break;
//                        }
//                    }
//                }
//            }
//            catch
//            {
//            }
//        }

//        public static StepInfoModel CreateStepInfo(StepNameType stepNameType)
//        {
//            List<StepParaModel> stepParaModelList = new List<StepParaModel>();
//            if (Config.Instance.dStepPara != null && Config.Instance.dStepPara.ContainsKey((int)stepNameType) && Config.Instance.dStepPara[(int)stepNameType].Count > 0)
//            {
//                for (int index = 0; index < Config.Instance.dStepPara[(int)stepNameType].Count; ++index)
//                    stepParaModelList.Add(new StepParaModel()
//                    {
//                        ParaName = Config.Instance.dStepPara[(int)stepNameType][index].ParaName,
//                        ParaType = Config.Instance.dStepPara[(int)stepNameType][index].ParaType,
//                        ParaValue = Config.Instance.dStepPara[(int)stepNameType][index].ParaValue,
//                        StepName = Config.Instance.dStepPara[(int)stepNameType][index].StepName,
//                        StepValue = Config.Instance.dStepPara[(int)stepNameType][index].StepValue
//                    });
//            }
//            return new StepInfoModel()
//            {
//                CurrentInterval = "",
//                TimeInterval = "",
//                VoltageInterval = "",
//                StepNameType = stepNameType,
//                CutConditionPara = new List<CutConditionParaModel>(),
//                StepPara = stepParaModelList
//            };
//        }

//        public static bool Connectok(string sHost)
//        {
//            if (!dNetSocket.ContainsKey(sHost))
//            {
//                dNetSocket.Add(sHost, new NetSocket());
//                dNetSocket[sHost].OnRead += new ReceiveData(ReadData);
//            }
//            if (!dNetSocket[sHost].state)
//                return dNetSocket[sHost].ConnectServer(sHost, 10020);
//            LogHelper.WriteLog(string.Format("连接设备{0}，而设备早已经连接。", sHost), p_ltType: LogType.Info);
//            return true;
//        }

//        public static void GetDeviceInfo(string sHost)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || !dNetSocket.ContainsKey(sHost))
//                return;
//            if (!dNetSocket[sHost].state)
//                dNetSocket[sHost].ConnectServer(sHost, 10020);
//            LogHelper.WriteLog(string.Format("获取设备信息，设备地址：{0}", sHost), p_ltType: LogType.Info);
//            dNetSocket[sHost].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//            {
//                Cmd = "0300"
//            }));
//        }

//        public static void GetStepInfo(string sHost, DeviceInfoModel device)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || !dNetSocket.ContainsKey(sHost))
//                return;
//            if (!dNetSocket[sHost].state)
//                dNetSocket[sHost].ConnectServer(sHost, 10020);
//            LogHelper.WriteLog(string.Format("获取工步信息，设备地址：{0}", sHost), p_ltType: LogType.Info);
//            dNetSocket[sHost].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//            {
//                Cmd = "0402",
//                Jstring = JsonHelper.SerializeObject((object)device)
//            }));
//        }

//        public static void SendMdCtr(string sHost, List<MdControlModel> Mdmodel)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || !dNetSocket.ContainsKey(sHost))
//                return;
//            if (!dNetSocket[sHost].state)
//                dNetSocket[sHost].ConnectServer(sHost, 10020);
//            LogHelper.WriteLog(string.Format("获取工步信息，设备地址：{0}", sHost), p_ltType: LogType.Info);
//            dNetSocket[sHost].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//            {
//                Cmd = "0403",
//                Jstring = JsonHelper.SerializeObject((object)Mdmodel)
//            }));
//        }

//        public static void Start(List<DeviceInfoModel> ctl)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || ctl == null || ctl.Count <= 0)
//                return;
//            Dictionary<string, List<DeviceInfoModel>> dictionary = new Dictionary<string, List<DeviceInfoModel>>();
//            foreach (DeviceInfoModel deviceInfoModel in ctl)
//            {
//                if (!dictionary.ContainsKey(deviceInfoModel.MasterIp))
//                    dictionary.Add(deviceInfoModel.MasterIp, new List<DeviceInfoModel>());
//                dictionary[deviceInfoModel.MasterIp].Add(deviceInfoModel);
//            }
//            foreach (string key in dictionary.Keys)
//            {
//                if (dNetSocket.ContainsKey(key))
//                {
//                    if (!dNetSocket[key].state)
//                        dNetSocket[key].ConnectServer(key, 10020);
//                    if (dNetSocket.ContainsKey(key))
//                    {
//                        LogHelper.WriteLog("启动：" + JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0301",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }), p_ltType: LogType.Info);
//                        dNetSocket[key].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0301",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }));
//                    }
//                }
//            }
//        }

//        public static void Stop(List<DeviceInfoModel> ctl)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || ctl == null || ctl.Count <= 0)
//                return;
//            Dictionary<string, List<DeviceInfoModel>> dictionary = new Dictionary<string, List<DeviceInfoModel>>();
//            foreach (DeviceInfoModel deviceInfoModel in ctl)
//            {
//                if (!dictionary.ContainsKey(deviceInfoModel.MasterIp))
//                    dictionary.Add(deviceInfoModel.MasterIp, new List<DeviceInfoModel>());
//                dictionary[deviceInfoModel.MasterIp].Add(new DeviceInfoModel()
//                {
//                    MasterIp = deviceInfoModel.MasterIp,
//                    Addr = deviceInfoModel.Addr,
//                    Chanel = deviceInfoModel.Chanel
//                });
//            }
//            foreach (string key in dictionary.Keys)
//            {
//                if (dNetSocket.ContainsKey(key))
//                {
//                    if (!dNetSocket[key].state)
//                        dNetSocket[key].ConnectServer(key, 10020);
//                    if (dNetSocket.ContainsKey(key))
//                    {
//                        LogHelper.WriteLog("停止：" + JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0302",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }), p_ltType: LogType.Info);
//                        dNetSocket[key].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0302",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }));
//                    }
//                }
//            }
//        }

//        public static void Pause(List<DeviceInfoModel> ctl)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || ctl == null || ctl.Count <= 0)
//                return;
//            Dictionary<string, List<DeviceInfoModel>> dictionary = new Dictionary<string, List<DeviceInfoModel>>();
//            foreach (DeviceInfoModel deviceInfoModel in ctl)
//            {
//                if (!dictionary.ContainsKey(deviceInfoModel.MasterIp))
//                    dictionary.Add(deviceInfoModel.MasterIp, new List<DeviceInfoModel>());
//                dictionary[deviceInfoModel.MasterIp].Add(new DeviceInfoModel()
//                {
//                    MasterIp = deviceInfoModel.MasterIp,
//                    Addr = deviceInfoModel.Addr,
//                    Chanel = deviceInfoModel.Chanel
//                });
//            }
//            foreach (string key in dictionary.Keys)
//            {
//                if (dNetSocket.ContainsKey(key))
//                {
//                    if (!dNetSocket[key].state)
//                        dNetSocket[key].ConnectServer(key, 10020);
//                    if (dNetSocket.ContainsKey(key))
//                    {
//                        LogHelper.WriteLog("暂停：" + JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0303",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }), p_ltType: LogType.Info);
//                        dNetSocket[key].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0303",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }));
//                    }
//                }
//            }
//        }

//        public static void Continue(List<DeviceInfoModel> ctl)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || ctl == null || ctl.Count <= 0)
//                return;
//            Dictionary<string, List<DeviceInfoModel>> dictionary = new Dictionary<string, List<DeviceInfoModel>>();
//            foreach (DeviceInfoModel deviceInfoModel in ctl)
//            {
//                if (!dictionary.ContainsKey(deviceInfoModel.MasterIp))
//                    dictionary.Add(deviceInfoModel.MasterIp, new List<DeviceInfoModel>());
//                dictionary[deviceInfoModel.MasterIp].Add(new DeviceInfoModel()
//                {
//                    MasterIp = deviceInfoModel.MasterIp,
//                    Addr = deviceInfoModel.Addr,
//                    Chanel = deviceInfoModel.Chanel
//                });
//            }
//            foreach (string key in dictionary.Keys)
//            {
//                if (dNetSocket.ContainsKey(key))
//                {
//                    if (!dNetSocket[key].state)
//                        dNetSocket[key].ConnectServer(key, 10020);
//                    if (dNetSocket.ContainsKey(key))
//                    {
//                        LogHelper.WriteLog("继续：" + JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0304",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }), p_ltType: LogType.Info);
//                        dNetSocket[key].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0304",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }));
//                    }
//                }
//            }
//        }

//        public static void Jump(List<DeviceInfoModel> ctl)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || ctl == null || ctl.Count <= 0)
//                return;
//            Dictionary<string, List<DeviceInfoModel>> dictionary = new Dictionary<string, List<DeviceInfoModel>>();
//            foreach (DeviceInfoModel deviceInfoModel in ctl)
//            {
//                if (!dictionary.ContainsKey(deviceInfoModel.MasterIp))
//                    dictionary.Add(deviceInfoModel.MasterIp, new List<DeviceInfoModel>());
//                dictionary[deviceInfoModel.MasterIp].Add(new DeviceInfoModel()
//                {
//                    MasterIp = deviceInfoModel.MasterIp,
//                    Addr = deviceInfoModel.Addr,
//                    Chanel = deviceInfoModel.Chanel,
//                    StartStep = deviceInfoModel.StartStep
//                });
//            }
//            foreach (string key in dictionary.Keys)
//            {
//                if (dNetSocket.ContainsKey(key))
//                {
//                    if (!dNetSocket[key].state)
//                        dNetSocket[key].ConnectServer(key, 10020);
//                    if (dNetSocket.ContainsKey(key))
//                    {
//                        LogHelper.WriteLog("跳转：" + JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0305",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }), p_ltType: LogType.Info);
//                        dNetSocket[key].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0305",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }));
//                    }
//                }
//            }
//        }

//        public static void Clear(List<DeviceInfoModel> ctl)
//        {
//            if (dNetSocket == null || dNetSocket.Values.Count <= 0 || ctl == null || ctl.Count <= 0)
//                return;
//            Dictionary<string, List<DeviceInfoModel>> dictionary = new Dictionary<string, List<DeviceInfoModel>>();
//            foreach (DeviceInfoModel deviceInfoModel in ctl)
//            {
//                if (!dictionary.ContainsKey(deviceInfoModel.MasterIp))
//                    dictionary.Add(deviceInfoModel.MasterIp, new List<DeviceInfoModel>());
//                dictionary[deviceInfoModel.MasterIp].Add(new DeviceInfoModel()
//                {
//                    MasterIp = deviceInfoModel.MasterIp,
//                    Addr = deviceInfoModel.Addr,
//                    Chanel = deviceInfoModel.Chanel
//                });
//            }
//            foreach (string key in dictionary.Keys)
//            {
//                if (dNetSocket.ContainsKey(key))
//                {
//                    if (!dNetSocket[key].state)
//                        dNetSocket[key].ConnectServer(key, 10020);
//                    if (dNetSocket.ContainsKey(key))
//                    {
//                        LogHelper.WriteLog("跳转：" + JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0306",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }), p_ltType: LogType.Info);
//                        dNetSocket[key].SendDataFromTcp(JsonHelper.SerializeObject((object)new CmdModel()
//                        {
//                            Cmd = "0306",
//                            Jstring = JsonHelper.SerializeObject((object)dictionary[key])
//                        }));
//                    }
//                }
//            }
//        }

//        public delegate void DeviceInformation(List<DeviceInfoModel> deviceInfo);

//        public delegate void StartReturnResultInfo(ReturnResult returnResult);

//        public delegate void StopReturnResultInfo(ReturnResult returnResult);

//        public delegate void PauseReturnResultInfo(ReturnResult returnResult);

//        public delegate void ContinueReturnResultInfo(ReturnResult returnResult);

//        public delegate void JumpReturnResultInfo(ReturnResult returnResult);

//        public delegate void AuxRealDataInfo(AuxRealDataModel auxRealData);

//        public delegate void RealDataModelInfo(RealDataModel realDataModel);

//        public delegate void DCIRModelInfo(DCIRModel realDataModel);

//        public delegate void VoltModelInfo(VoltModel voltDataModel);

//        public delegate void StepDataInfo(StepInfoMessage stepDataMessage);

//        public delegate void BmsRealDataInfo(BmsRealDataModel bmsDataModel);

//        public delegate void ClearReturnResultInfo(ReturnResult returnResult);

//        public delegate void MdCmdReturnResultInfo(ReturnResult returnResult);
//    }
//}
