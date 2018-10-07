<template>
    <div class="ui middle aligned center aligned grid">
        <div class="column">
            <h2 class="ui teal image header">                
                    Log-in to system                
            </h2>
            <form class="ui large form">
                <div class="ui stacked segment">
                    <div class="field">
                        <div class="ui left icon input">
                            <i class="user icon"></i>
                            <input v-model="email" name="email" placeholder="E-mail address" type="text">
                        </div>
                    </div>
                    <div class="field">
                        <div class="ui left icon input">
                            <i class="lock icon"></i>
                            <input v-model="password" name="password"  placeholder="Password" type="password">
                        </div>
                    </div>
                    <div class="ui fluid large teal submit button" v-on:click="login">Login</div>
                </div>
    
                <div class="ui error message"></div>
    
            </form>
    
            <div class="ui message">
                New to us? <a href="#">Sign Up</a>
            </div>
        </div>
    </div>
</template>

<script>
import { AUTH_REQUEST } from './../store/auth/actions'

export default {
    name: 'login',
    
    data () {
        return {
            email: 'dogo',
            password: 'dogy',
        }
    },
    
    methods: {
        login: function () {
            const { email, password } = this;
            this.$store.dispatch(AUTH_REQUEST, { email, password }).then(() => {
            this.$router.push('/')
        })
    }
  },
}
</script>

<style scoped>
</style>