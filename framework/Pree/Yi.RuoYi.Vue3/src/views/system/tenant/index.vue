<template>

  <div class="app-container">
    <!-- <div>
该文件为通用Crud模板文件，按照规范只需要 替换以下变量即可，
租户 ： 实体中文名称
system:tenant ：crud权限编码
system/tenant : api文件路径,例如：codeGen/tableApi
</div> -->
    <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="100px">
      <el-form-item label="租户名称" prop="name">
        <el-input v-model="queryParams.name" placeholder="请输入租户名称" clearable style="width: 240px"
                  @keyup.enter="handleQuery" prop="name"/>
      </el-form-item>
      <!-- <el-form-item label="状态" prop="isDeleted">
      <el-select
        v-model="queryParams.isDeleted"
        placeholder="状态"
        clearable
        style="width: 240px"
      >
        <el-option
          v-for="dict in sys_normal_disable"
          :key="dict.value"
          :label="dict.label"
          :value="dict.value"
        />
      </el-select>
    </el-form-item> -->
      <el-form-item label="创建时间" style="width: 308px">
        <el-date-picker
            v-model="dateRange"
            value-format="YYYY-MM-DD"
            type="daterange"
            range-separator="-"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
        ></el-date-picker>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" icon="Search" @click="handleQuery">搜索</el-button>
        <el-button icon="Refresh" @click="resetQuery">重置</el-button>
      </el-form-item>
    </el-form>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button type="primary" plain icon="Plus" @click="handleAdd" v-hasPermi="['system:tenant:add']">新增
        </el-button>
      </el-col>

      <el-col :span="1.5">
        <el-button type="success" plain icon="Edit" :disabled="single" @click="handleUpdate"
                   v-hasPermi="['system:tenant:edit']">修改
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="danger" plain icon="Delete" :disabled="multiple" @click="handleDelete"
                   v-hasPermi="['system:tenant:remove']">删除
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="warning" plain icon="Download" @click="handleExport"
                   v-hasPermi="['system:tenant:export']">导出
        </el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <el-table v-loading="loading" :data="dataList" @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55" align="center"/>

      <!-----------------------这里开始就是数据表单的全部列------------------------>
      <el-table-column label="租户编号" align="center" prop="id" width="380"/>

      <el-table-column label="租户名称" align="center" prop="name" :show-overflow-tooltip="true"/>
      <el-table-column label="连接字符串" align="center" prop="tenantConnectionString" :show-overflow-tooltip="true"/>

      <el-table-column label="数据库类型" align="center" prop="dbType" :show-overflow-tooltip="true"/>
      <el-table-column
          label="创建时间"
          align="center"
          prop="creationTime"
          width="180"
      >
        <template #default="scope">
          <span>{{ parseTime(scope.row.creationTime) }}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="状态" align="center" prop="isDeleted">
      <template #default="scope">
        <dict-tag
          :options="sys_normal_disable"
          :value="scope.row.isDeleted"
        />
      </template>
    </el-table-column> -->
      <!-- <el-table-column
      label="备注"
      align="center"
      prop="remark"
      :show-overflow-tooltip="true"
    /> -->

      <el-table-column label="操作" align="center" class-name="small-padding fixed-width">
        <template #default="scope">

          <el-button link icon="Edit" @click="handleInit(scope.row.id)"
                     v-hasPermi="['system:tenant:edit']">初始化
          </el-button>
          <el-button link icon="Edit" @click="handleUpdate(scope.row)"
                     v-hasPermi="['system:tenant:edit']">修改
          </el-button>
          <el-button link icon="Delete" @click="handleDelete(scope.row)"
                     v-hasPermi="['system:tenant:remove']">删除
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total > 0" :total="Number(total)" v-model:page="queryParams.pageNum"
                v-model:limit="queryParams.pageSize" @pagination="getList"/>

    <!-- ---------------------这里是新增和更新的对话框--------------------- -->
    <el-dialog :title="title" v-model="open" width="600px" append-to-body>
      <el-form ref="dataRef" :model="form" :rules="rules" label-width="100px">


        <el-form-item label="租户名称" prop="name">
          <el-input v-model="form.name" placeholder="请输入租户名称"/>
        </el-form-item>

        <el-form-item label="数据库类型" prop="dbType">
          <el-radio-group v-model="form.dbType">
            <el-radio
                v-for="dict in ['Sqlite','Mysql','Sqlserver']"
                :key="dict"
                :value="dict"
            >{{ dict }}
            </el-radio
            >
          </el-radio-group>
        </el-form-item>
        <el-form-item label="连接字符串" prop="tenantConnectionString">
          <el-input v-model="form.tenantConnectionString" type="textarea" placeholder="请输入内容"></el-input>
        </el-form-item>
        <h3>连接字符串示例：</h3>
        <p>Sqlite: DataSource=[xxxx]</p>
        <p>Mysql: server=[xxxx];port=3306;database=[xxxx];user id=[xxxx];password=[xxxx]</p>
        <p>Sqlserver: Data Source=[xxxx];Initial Catalog=[xxxx];User ID=[xxxx];password=[xxxx]</p>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button type="primary" @click="submitForm">确 定</el-button>
          <el-button @click="cancel">取 消</el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import {
  listData,
  getData,
  delData,
  addData,
  updateData,
  InitData,
  getExportExcel
} from "@/api/system/tenant";
import {ref} from "@vue/reactivity";


const {proxy} = getCurrentInstance();
const {sys_normal_disable} = proxy.useDict("sys_normal_disable");

const dataList = ref([]);
const open = ref(false);
const loading = ref(true);
const showSearch = ref(true);
const ids = ref([]);
const single = ref(true);
const multiple = ref(true);
const total = ref(0);
const title = ref("");
const dateRange = ref([]);
const data = reactive({
  form: {},
  queryParams: {
    pageNum: 1,
    pageSize: 10,
    name: undefined,
    code: undefined,
  },
  rules: {
    code: [{required: true, message: "租户编号不能为空", trigger: "blur"}],
    name: [{required: true, message: "租户名称不能为空", trigger: "blur"}],
  },
});


const {queryParams, form, rules} = toRefs(data);

/** 查询列表 */
function getList() {
  loading.value = true;
  listData(proxy.addDateRange(queryParams.value, dateRange.value)).then(
      (response) => {
        dataList.value = response.data.items;
        total.value = response.data.totalCount;
        loading.value = false;
      }
  );
}

/** 取消按钮 */
function cancel() {
  open.value = false;
  reset();
}


/** 表单重置 */
function reset() {
  form.value = {};
  proxy.resetForm("dataRef");
}

/** 搜索按钮操作 */
function handleQuery() {
  queryParams.value.pageNum = 1;
  getList();
}

/** 重置按钮操作 */
function resetQuery() {
  dateRange.value = [];
  proxy.resetForm("queryRef");
  handleQuery();
}

/** 新增按钮操作 */
function handleAdd() {
  reset();
  open.value = true;
  title.value = "添加租户";
}

/** 多选框选中数据 */
function handleSelectionChange(selection) {
  ids.value = selection.map((item) => item.id);
  single.value = selection.length != 1;
  multiple.value = !selection.length;
}

/** 修改按钮操作 */
function handleUpdate(row) {
  reset();
  const id = row.id || ids.value;
  getData(id).then((response) => {
    form.value = response.data;
    open.value = true;
    title.value = "修改租户";
  });
}

/** 初始化按钮操作 */
function handleInit(id) {
  InitData(id).then((response) => {
    proxy.$modal.msgSuccess("初始化成功");
    getList();
  });
}

/** 提交按钮 */
function submitForm() {
  proxy.$refs["dataRef"].validate((valid) => {
    if (valid) {
      if (form.value.id != undefined) {
        updateData(form.value.id, form.value).then((response) => {
          proxy.$modal.msgSuccess("修改成功");
          open.value = false;
          getList();
        });
      } else {
        addData(form.value).then((response) => {
          proxy.$modal.msgSuccess("新增成功");
          open.value = false;
          getList();
        });
      }
    }
  });
}

/** 删除按钮操作 */
function handleDelete(row) {
  const delIds = row.id || ids.value;
  proxy.$modal
      .confirm('是否确认删除编号为"' + delIds + '"的数据项？')
      .then(function () {
        return delData(delIds);
      })
      .then(() => {
        getList();
        proxy.$modal.msgSuccess("删除成功");
      })
      .catch(() => {
      });
}

/** 导出按钮操作 */
function handleExport() {
  getExportExcel(proxy.addDateRange(queryParams.value, dateRange.value));

  // proxy.download("/system/tenant/export-excel", {
  //     ...proxy.addDateRange(queryParams.value, dateRange.value)
  // }, `tenant_${new Date().getTime()}.xlsx`);
}

getList();
</script>
