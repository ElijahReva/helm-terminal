    <template lang="html">
    <div>
        <Header title="Helm Terminal"/>
        <MainMenu />
        <TabView />
    </div>    
</template>

<script>
import Header from './components/Header.vue'
import TabView from './components/TabView.vue'
import MainMenu from './components/Menu.vue'

export default {
    name: 'App',
    components:{
        Header,
        TabView,
        MainMenu,
    }
}
</script>


<style scoped>

</style>
