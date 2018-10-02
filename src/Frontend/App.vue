    <template lang="html">
    <div>
        <Header title="Helm Terminal"/>
        <TabView />
    </div>    
</template>

<script>
import Header from './components/Header.vue'
import TabView from './components/TabView.vue'
import {INIT_CONNECTION} from "./store/rootConstants";


export default {
    name: 'App',
    components:{
        Header,
        TabView,
    },
    created() {
        this.$store.dispatch(INIT_CONNECTION).catch(console.log);
    }
}
</script>


<style scoped>

</style>
