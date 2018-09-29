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
    export default {
        name: "ChartList",
        data() {
            return {
                charts: [
                    { name: "env-man", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-dash", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                    { name: "sym-identity", revision: 323, status: "Deployed", date: Date.now() },
                ]
            }
        },
    }
</script>

<style scoped>

</style>