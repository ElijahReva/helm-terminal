<template>
    <table class="ui selectable table">
        <thead>
            <tr>
                <th>Name</th>
                <th>Chart</th>
                <th>Revision</th>
                <th>Status</th>
                <th>updated</th>
            </tr>
        </thead>
        <tbody>
            <tr 
                v-for="chart in getCharts" 
                :key="chart.name"
                @click="SELECT_CHART(chart.name)"
                v-bind:class="{ selected: chart.isSelected}" >
                <td>{{ chart.name }}</td>
                <td>{{ chart.chart }}</td>
                <td>{{ chart.revision }}</td>
                <td>{{ chart.status }}</td>
                <td>{{ chart.updated }}</td>
            </tr>
        </tbody>        
    </table>
</template>

<script>    
import {
    SELECT_CHART
} from "../../store/manager/actions";

import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions} = createNamespacedHelpers('manager');

export default {
    name: "ChartList",
    
    methods: {
        ...mapActions([
            SELECT_CHART,
        ]),
    },
    
    computed: {
        ...mapGetters([ 
            'getCharts',
        ]),
        
        selectedChart: {
            get() { return this.$store.state.manager.selectedChart; },
            set(value) { this.SELECT_CHART(value); },
        },
    }
}
</script>

<style scoped>
tr.selected {
    background: rgba(0,0,0,.15) !important;
}
</style>