<template>
  <div class="app-container">
    <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="68px">
      <el-form-item label="字典名称" prop="dictName">
        <el-input
            v-model="queryParams.dictName"
            placeholder="请输入字典名称"
            clearable
            style="width: 240px"
            @keyup.enter="handleQuery"
        />
      </el-form-item>
      <el-form-item label="字典类型" prop="dictType">
        <el-input
            v-model="queryParams.dictType"
            placeholder="请输入字典类型"
            clearable
            style="width: 240px"
            @keyup.enter="handleQuery"
        />
      </el-form-item>
      <el-form-item label="状态" prop="state">
        <el-select
            v-model="queryParams.state"
            placeholder="字典状态"
            clearable
            style="width: 240px"
        >
          <el-option
              v-for="dict in sys_normal_disable"
              :key="JSON.parse(dict.value)"
              :label="dict.label"
              :value="JSON.parse(dict.value)"
          />
        </el-select>
      </el-form-item>
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
        <el-button
            type="primary"
            plain
            icon="Plus"
            @click="handleAdd"
            v-hasPermi="['system:dict:add']"
        >新增
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button
            type="success"
            plain
            icon="Edit"
            :disabled="single"
            @click="handleUpdate"
            v-hasPermi="['system:dict:edit']"
        >修改
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button
            type="danger"
            plain
            icon="Delete"
            :disabled="multiple"
            @click="handleDelete"
            v-hasPermi="['system:dict:remove']"
        >删除
        </el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <el-table v-loading="loading" :data="typeList" @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55" align="center"/>
      <el-table-column label="字典编号" align="center" prop="id"/>
      <el-table-column label="字典名称" align="center" prop="dictName" :show-overflow-tooltip="true"/>
      <el-table-column label="字典类型" align="center" :show-overflow-tooltip="true">
        <template #default="scope">
          <router-link :to="'/system/dict-data/index/' + scope.row.id" class="link-type">
            <span>{{ scope.row.dictType }}</span>
          </router-link>
          <!-- <span style="color: blue; cursor: pointer; " @click="handleToPath(scope.row.id)">{{ scope.row.dictType }}</span> -->
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center" prop="state">
        <template #default="scope">
          <dict-tag :options="sys_normal_disable" :value="scope.row.state"/>
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center" prop="remark" :show-overflow-tooltip="true"/>
      <el-table-column label="创建时间" align="center" prop="creationTime" width="180">
        <template #default="scope">
          <span>{{ parseTime(scope.row.creationTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" class-name="small-padding fixed-width">
        <template #default="scope">
          <el-button
              link
              icon="Edit"
              @click="handleUpdate(scope.row)"
              v-hasPermi="['system:dict:edit']"
          >修改
          </el-button>
          <el-button
              link
              icon="Delete"
              @click="handleDelete(scope.row)"
              v-hasPermi="['system:dict:remove']"
          >删除
          </el-button>
        </template>
      </el-table-column>
    </el-table>


    <pagination
        v-show="total > 0"
        :total="Number(total)"
        v-model:page="queryParams.pageNum"
        v-model:limit="queryParams.pageSize"
        @pagination="getList"
    />

    <!-- 添加或修改参数配置对话框 -->
    <el-dialog :title="title" v-model="open" width="500px" append-to-body>
      <el-form ref="dictRef" :model="form" :rules="rules" label-width="80px">
        <el-form-item label="字典名称" prop="dictName">
          <el-input v-model="form.dictName" placeholder="请输入字典名称"/>
        </el-form-item>
        <el-form-item label="字典类型" prop="dictType">
          <el-input v-model="form.dictType" placeholder="请输入字典类型"/>
        </el-form-item>
        <el-form-item label="状态" prop="state">
          <el-radio-group v-model="form.state">
            <el-radio v-for="dict in sys_normal_disable" :key="dict.value" :value="JSON.parse(dict.value)">
              {{ dict.label }}
            </el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="备注" prop="remark">
          <el-input v-model="form.remark" type="textarea" placeholder="请输入内容"></el-input>
        </el-form-item>
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

<script setup name="Dict">
import useDictStore from '@/store/modules/dict'
import {listType, getType, delType, addType, updateType} from "@/api/system/dict/type";
import router from '../../../router';

const {proxy} = getCurrentInstance();
const {sys_normal_disable} = proxy.useDict("sys_normal_disable");

const typeList = ref([]);
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
    dictName: undefined,
    dictType: undefined,
    state: true
  },
  rules: {
    dictName: [{required: true, message: "字典名称不能为空", trigger: "blur"}],
    dictType: [{required: true, message: "字典类型不能为空", trigger: "blur"}]
  },
});

const {queryParams, form, rules} = toRefs(data);

/** 查询字典类型列表 */
function getList() {
  loading.value = true;
  listType(proxy.addDateRange(queryParams.value, dateRange.value)).then(response => {
    typeList.value = response.data.items;
    total.value = response.data.totalCount;
    loading.value = false;
  });
}

/** 取消按钮 */
function cancel() {
  open.value = false;
  reset();
}

/** 表单重置 */
function reset() {

  proxy.resetForm("dictRef");
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
  title.value = "添加字典类型";
}

/** 多选框选中数据 */
function handleSelectionChange(selection) {
  ids.value = selection.map(item => item.id);
  single.value = selection.length != 1;
  multiple.value = !selection.length;
}

/** 修改按钮操作 */
function handleUpdate(row) {
  reset();
  const dictId = row.id || ids.value;
  getType(dictId).then(response => {
    form.value = response.data;
    open.value = true;
    title.value = "修改字典类型";
  });
}

/** 提交按钮 */
function submitForm() {
  proxy.$refs["dictRef"].validate(valid => {
    if (valid) {
      if (form.value.id != undefined) {
        updateType(form.value).then(response => {
          proxy.$modal.msgSuccess("修改成功");
          open.value = false;
          getList();
        });
      } else {
        addType(form.value).then(response => {
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
  const dictIds = row.id || ids.value;
  proxy.$modal.confirm('是否确认删除字典编号为"' + dictIds + '"的数据项？').then(function () {
    return delType(dictIds);
  }).then(() => {
    getList();
    proxy.$modal.msgSuccess("删除成功");
  }).catch(() => {
  });
}

getList();

const handleToPath = (id) => {
  router.push({
    path: '/system/dict-data/index',
    query: {
      dictId: id
    }
  })
}
</script>
