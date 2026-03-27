<template>
  <div class="app-container">
    <el-form :model="queryParams" ref="queryRef" :inline="true" v-show="showSearch" label-width="80px">
      <el-form-item label="语言名称" prop="name">
        <el-input
            v-model="queryParams.name"
            placeholder="请输入语言名称"
            clearable
            style="width: 240px"
            @keyup.enter="handleQuery"
        />
      </el-form-item>
      <el-form-item label="语言值" prop="value">
        <el-input
            v-model="queryParams.value"
            placeholder="请输入语言值"
            clearable
            style="width: 240px"
            @keyup.enter="handleQuery"
        />
      </el-form-item>
      <el-form-item label="语言区域" prop="culture">
        <el-input
            v-model="queryParams.culture"
            placeholder="请输入语言区域，如：zh-CN"
            clearable
            style="width: 240px"
            @keyup.enter="handleQuery"
        />
      </el-form-item>
      <el-form-item label="创建时间" style="width: 320px">
        <el-date-picker
            v-model="dateRange"
            value-format="YYYY-MM-DD"
            type="daterange"
            range-separator="-"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
        />
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
            v-hasPermi="['system:language:add']"
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
            v-hasPermi="['system:language:edit']"
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
            v-hasPermi="['system:language:remove']"
        >删除
        </el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList"></right-toolbar>
    </el-row>

    <el-table v-loading="loading" :data="languageList" @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55" align="center"/>
      <el-table-column label="编号" align="center" prop="id" width="240"/>
      <el-table-column label="语言名称" align="center" prop="name" min-width="180" :show-overflow-tooltip="true"/>
      <el-table-column label="语言值" align="center" prop="value" min-width="180" :show-overflow-tooltip="true"/>
      <el-table-column label="语言区域" align="center" prop="culture" width="160"/>
      <el-table-column label="创建人" align="center" prop="creatorId" width="260">
        <template #default="scope">
          <span>{{ scope.row.creatorId || '-' }}</span>
        </template>
      </el-table-column>
      <el-table-column label="创建时间" align="center" prop="creationTime" width="180">
        <template #default="scope">
          <span>{{ parseTime(scope.row.creationTime) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" width="160" class-name="small-padding fixed-width">
        <template #default="scope">
          <el-button
              link
              icon="Edit"
              @click="handleUpdate(scope.row)"
              v-hasPermi="['system:language:edit']"
          >修改
          </el-button>
          <el-button
              link
              icon="Delete"
              @click="handleDelete(scope.row)"
              v-hasPermi="['system:language:remove']"
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

    <el-dialog :title="title" v-model="open" width="620px" append-to-body>
      <el-form ref="languageRef" :model="form" :rules="rules" label-width="100px">
        <el-form-item label="语言名称" prop="name">
          <el-input v-model="form.name" placeholder="请输入语言名称"/>
        </el-form-item>
        <el-form-item label="语言值" prop="value">
          <el-input v-model="form.value" placeholder="请输入语言值"/>
        </el-form-item>
        <el-form-item label="语言区域" prop="culture">
          <el-input v-model="form.culture" placeholder="请输入语言区域，如：zh-CN"/>
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

<script setup name="Language">
import {addLanguage, delLanguage, getLanguage, listLanguage, updateLanguage} from '@/api/system/language'

const {proxy} = getCurrentInstance()

const languageList = ref([])
const open = ref(false)
const loading = ref(true)
const showSearch = ref(true)
const ids = ref([])
const single = ref(true)
const multiple = ref(true)
const total = ref(0)
const title = ref('')
const dateRange = ref([])

const data = reactive({
  form: {},
  queryParams: {
    pageNum: 1,
    pageSize: 10,
    name: undefined,
    value: undefined,
    culture: undefined
  },
  rules: {
    name: [{required: true, message: '语言名称不能为空', trigger: 'blur'}],
    value: [{required: true, message: '语言值不能为空', trigger: 'blur'}],
    culture: [{required: true, message: '语言区域不能为空', trigger: 'blur'}]
  }
})

const {queryParams, form, rules} = toRefs(data)

function getList() {
  loading.value = true
  listLanguage(proxy.addDateRange(queryParams.value, dateRange.value)).then(response => {
    languageList.value = response.data.items
    total.value = response.data.totalCount
    loading.value = false
  }).catch(() => {
    loading.value = false
  })
}

function cancel() {
  open.value = false
  reset()
}

function reset() {
  form.value = {
    id: undefined,
    name: undefined,
    value: undefined,
    culture: undefined
  }
  proxy.resetForm('languageRef')
}

function handleQuery() {
  queryParams.value.pageNum = 1
  getList()
}

function resetQuery() {
  dateRange.value = []
  proxy.resetForm('queryRef')
  handleQuery()
}

function handleSelectionChange(selection) {
  ids.value = selection.map(item => item.id)
  single.value = selection.length != 1
  multiple.value = !selection.length
}

function handleAdd() {
  reset()
  open.value = true
  title.value = '添加语言'
}

function handleUpdate(row) {
  reset()
  const languageId = row.id || ids.value
  getLanguage(languageId).then(response => {
    form.value = response.data
    open.value = true
    title.value = '修改语言'
  })
}

function submitForm() {
  proxy.$refs['languageRef'].validate(valid => {
    if (valid) {
      if (form.value.id != undefined) {
        updateLanguage(form.value.id, form.value).then(() => {
          proxy.$modal.msgSuccess('修改成功')
          open.value = false
          getList()
        })
      } else {
        addLanguage(form.value).then(() => {
          proxy.$modal.msgSuccess('新增成功')
          open.value = false
          getList()
        })
      }
    }
  })
}

function handleDelete(row) {
  const languageIds = row.id || ids.value
  proxy.$modal.confirm('是否确认删除语言编号为"' + languageIds + '"的数据项？').then(function () {
    return delLanguage(languageIds)
  }).then(() => {
    getList()
    proxy.$modal.msgSuccess('删除成功')
  }).catch(() => {
  })
}

getList()
</script>
