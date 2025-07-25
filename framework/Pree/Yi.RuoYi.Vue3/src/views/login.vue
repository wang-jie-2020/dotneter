<template>
  <div class="login">
    <el-form ref="loginRef" :model="loginForm" :rules="loginRules" class="login-form">
      <h3 class="title">意框架-Ruoyi管理系统</h3>
      <el-form-item prop="username">
        <el-input v-model="loginForm.username" type="text" size="large" auto-complete="off"
                  :placeholder="$t('account')">
          <template #prefix>
            <svg-icon icon-class="user" class="el-input__icon input-icon"/>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item prop="password">
        <el-input v-model="loginForm.password" type="password" size="large" auto-complete="off"
                  :placeholder="$t('password')"
                  @keyup.enter="handleLogin">
          <template #prefix>
            <svg-icon icon-class="password" class="el-input__icon input-icon"/>
          </template>
        </el-input>
      </el-form-item>
      <el-form-item prop="code" v-if="captchaEnabled">
        <el-input v-model="loginForm.code" size="large" auto-complete="off" :placeholder="$t('verification code')"
                  style="width: 63%"
                  @keyup.enter="handleLogin">
          <template #prefix>
            <svg-icon icon-class="validCode" class="el-input__icon input-icon"/>
          </template>
        </el-input>
        <div class="login-code">
          <img :src="codeUrl" @click="getCode" class="login-code-img"/>
        </div>
      </el-form-item>
      <el-form-item>
        <span>当前租户：</span>
        <el-select v-model="tenantSelected" class="m-2" placeholder="租户选择" style="width: 80%">
          <el-option v-for="item in tenantList" :key="item.id" :label="item.name" :value="item.name"/>
        </el-select>
      </el-form-item>

      <el-checkbox v-model="loginForm.rememberMe" style="margin:0px 0px 25px 0px;">记住密码</el-checkbox>
      <el-form-item style="width:100%;">
        <div class="lang" style="flex: 1">
          <el-dropdown @command="handleLang">
            <span class="el-dropdown-link">
              <img src="@/assets/icons/svg/language.svg" alt=""/>
            </span>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item :disabled="app.lang == 'zh'" command="zh">中文</el-dropdown-item>
                <el-dropdown-item :disabled="app.lang == 'en'" command="en">English</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
        </div>
        <el-button :loading="loading" size="large" type="primary" style="width: 100%; flex: 9; margin-right: 5px"
                   @click.prevent="handleLogin">
          <span v-if="!loading">登 录</span>
          <span v-else>登 录 中...</span>
        </el-button>
        <div style="float: right;" v-if="register">
          <router-link class="link-type" :to="'/register'">立即注册</router-link>
        </div>
      </el-form-item>
    </el-form>
    <!--  底部  -->
    <div class="el-login-footer">
      <span>Copyright © 2018-2023 ruoyi.vip All Rights Reserved.</span>
    </div>
  </div>
</template>

<script setup>
import {getCodeImg} from "@/api/login";
import Cookies from "js-cookie";
import {encrypt, decrypt} from "@/utils/jsencrypt";
import useUserStore from '@/store/modules/user'
import {SelectData as getTenantList} from '@/api/system/tenant'
import {ref} from "vue";
import useAppStore from "@/store/modules/app";
import {useI18n} from "vue-i18n";

const userStore = useUserStore()
const router = useRouter();
const {proxy} = getCurrentInstance();

const {locale} = useI18n();
const app = useAppStore();
const lang = computed(() => useAppStore().lang);

const tenantSelected = ref('defalut');
const loginForm = ref({
  username: "",
  password: "",
  rememberMe: false,
  code: "",
  uuid: "",
});

const loginRules = {
  username: [{required: true, trigger: "blur", message: "请输入您的账号"}],
  password: [{required: true, trigger: "blur", message: "请输入您的密码"}],
  code: [{required: true, trigger: "change", message: "请输入验证码"}]
};

const codeUrl = ref("");
const loading = ref(false);
// 验证码开关
const captchaEnabled = ref(true);
// 注册开关
const register = ref(false);
const redirect = ref(undefined);
const tenantList = ref([]);

function handleLang(command) {
  locale.value = command;
  app.lang = command;
  localStorage.setItem("lang", command);
  location.reload();
}

function handleLogin() {
  proxy.$refs.loginRef.validate(valid => {
    if (valid) {
      loading.value = true;
      // 勾选了需要记住密码设置在 cookie 中设置记住用户名和密码
      if (loginForm.value.rememberMe) {
        Cookies.set("username", loginForm.value.username, {expires: 30});
        Cookies.set("password", encrypt(loginForm.value.password), {expires: 30});
        Cookies.set("rememberMe", loginForm.value.rememberMe, {expires: 30});
      } else {
        // 否则移除
        Cookies.remove("username");
        Cookies.remove("password");
        Cookies.remove("rememberMe");
      }
      // 调用action的登录方法
      const currentTenantId = tenantList.value.filter(x => x.name == tenantSelected.value)[0]?.id ?? null;

      console.log(currentTenantId, 'currentTenantId')
      userStore.login(loginForm.value, currentTenantId).then(() => {
        router.push({path: redirect.value || "/"});
      }).catch(() => {
        loading.value = false;

        // 重新获取验证码
        if (captchaEnabled.value) {
          getCode();
        }
      });
    }
  });
}

function getCode() {

  getCodeImg().then(res => {
    captchaEnabled.value = res.captchaEnabled === undefined ? true : res.captchaEnabled;
    if (captchaEnabled.value) {
      codeUrl.value = "data:image/gif;base64," + res.data.img;
      loginForm.value.uuid = res.data.uuid;
    }
  });
}

function getCookie() {
  const username = Cookies.get("username");
  const password = Cookies.get("password");
  const rememberMe = Cookies.get("rememberMe");
  loginForm.value = {
    username: username === undefined ? loginForm.value.username : username,
    password: password === undefined ? loginForm.value.password : decrypt(password),
    rememberMe: rememberMe === undefined ? false : Boolean(rememberMe)
  };
}

async function getTenant() {
  const {data} = await getTenantList();
  tenantList.value = [{name: "defalut"}, ...data];
}

getCode();
getCookie();
getTenant();
</script>

<style lang='scss' scoped>
.login {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  background-image: url("../assets/images/login-background.jpg");
  background-size: cover;
}

.title {
  margin: 0px auto 30px auto;
  text-align: center;
  color: #707070;
}

.login-form {
  border-radius: 6px;
  background: #ffffff;
  width: 400px;
  padding: 25px 25px 5px 25px;

  .el-input {
    height: 40px;

    input {
      height: 40px;
    }
  }

  .input-icon {
    height: 39px;
    width: 14px;
    margin-left: 0px;
  }
}

.login-tip {
  font-size: 13px;
  text-align: center;
  color: #bfbfbf;
}

.login-code {

  width: 33%;
  height: 40px;
  margin-left: auto;
  border: 1px solid #DCDFE6 !important;
  border-radius: 4px;

  img {

    cursor: pointer;
    vertical-align: middle;
  }
}

.el-login-footer {
  height: 40px;
  line-height: 40px;
  position: fixed;
  bottom: 0;
  width: 100%;
  text-align: center;
  color: #fff;
  font-family: Arial;
  font-size: 12px;
  letter-spacing: 1px;
}

.login-code-img {
  height: 100%;
  width: 100%;
}

.lang {
  padding-top: 12px;
  margin-right: 5px;

  img {
    width: 22px;
    height: 22px;
  }
}
</style>
