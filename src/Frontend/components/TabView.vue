<template lang="html">
    <div class="ui container">
        <div class="ui grid">
            <div class="four wide column">
                <div class="ui vertical fluid tabular menu">
                    <template v-for="tab in tabs">
                        <router-link class="item" v-bind:class="{ active: tab.isActive }" :to="tab.path">{{ tab.title }}</router-link>
                    </template>
                </div>
            </div>
            <div class="twelve wide stretched column">
                <div class="ui segment">
                    <router-view class="view"></router-view>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

import JQuery from 'jquery'
let $ = JQuery;
import { mapState, mapActions, mapGetters } from 'vuex'

export default {
    name: 'TabView',
    computed: mapGetters('nav', ['tabs']),    
}
</script>

<style scoped>

</style>