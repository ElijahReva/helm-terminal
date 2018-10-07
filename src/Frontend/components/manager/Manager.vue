<template lang="html">
    <div class="ui grid container">
        <div class="sixteen wide column">
            <ActionMenu/>
        </div>
        <div class="sixteen wide column">
            <ChartList />
        </div>
    </div>       
</template>


<script>
    import { createNamespacedHelpers, mapState } from 'vuex'
    import ActionMenu from "./ActionMenu";
    import ChartList from "./ChartList";
    import JQuery from 'jquery'
    import {UPDATE_CHARTS} from "../../store/manager/actions";
    let $ = JQuery;
    
    const { mapActions } = createNamespacedHelpers('manager');
        
    export default {
        name: 'Main',
        
        components: {ChartList, ActionMenu},
        
        methods: {
            ...mapActions([
                UPDATE_CHARTS
            ]),
        },
        computed: {
            ...mapState([
                'currentNamespace',
            ]),
        },
        mounted(){
            
            this.UPDATE_CHARTS()
                .then(result => {

                    console.log(result);
                })
                .catch(err => console.log(err));
        },
        
        watch: {
            currentNamespace: function(event) {

                this.UPDATE_CHARTS()
                    .then(result => {

                        console.log(result);
                    })
                    .catch(err => console.log(err));
            }
        }
    }
</script>
<style scoped>
.overlay {
  float: left;
  margin: 0 3em 1em 0;
}
.overlay .menu {
  position: relative;
  left: 0;
  transition: left 0.5s ease;
}
.overlay.fixed .menu {
  left: 800px;
}

.text.container .left.floated.image {
  margin: 2em 2em 2em -4em;
}
.text.container .right.floated.image {
  margin: 2em -4em 2em 2em;
}
</style>