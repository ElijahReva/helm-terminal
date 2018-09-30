<template>
    <div class="sixteen wide column">
        <div class="ui middle aligned divided list">
            <template v-for="chart in charts">
                <div class="item">
                    <img class="ui avatar image" src="https://loremflickr.com/320/240">
                    <div class="content">
                        {{ chart.name }}
                    </div>
                </div>
            </template>
        </div>
    </div>
</template>

<script>    
    import { mapGetters, mapState } from 'vuex'
    export default {
        name: "ChartList",
        computed: {
            ...mapState('manager', [ 'charts' ])
        }
    }
</script>

<style scoped>

</style>