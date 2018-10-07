<!--suppress ALL -->
<template lang="html">
  <div class="ui inverted borderless main menu">
    <div class="ui container">
      <div class="right menu">
        <div class="ui item inline loader"  v-if="isRequestInProgress" />
        <div class="ui item left aligned header">{{ title }}</div>
        <div class="item">Kubernetes Context</div>
        <select v-model="currentContext" class="ui item search selection dropdown">
              <option
                  v-for="context in contexts"
                  :value="context.name"
                  :key="context.name"
                  class="item">
                
                  {{ context.name }}
                
              </option >
        </select>        
        <select v-model="currentNamespace" id="namespace-dropbox" class="ui item search selection dropdown">
          <option
                  v-for="ns in namespaces"
                  :value="ns.name"
                  :key="ns.name"
                  class="item">
            
            {{ ns.name }}

          </option >
        </select>        
        <div class="item">Kubernetes Namespace</div>
      </div>
    </div>
  </div>
</template>
<script>
    import { mapState, mapActions, mapMutations } from 'vuex'
    import { 
        REQUEST_CONTEXTS,
        SET_CONTEXT,
        SET_NAMESPACE,
        UPDATE_NAMESPACES,
        REQUEST_NAMESPACES,
    } from "../store/rootConstants";
    import JQuery from 'jquery'
    
    let $ = JQuery;    

    export default {
        name: 'Header',
        props: [ 'title' ],
        computed: {
            
            currentNamespace: {
                get() { return this.$store.state.currentNamespace; },
                set(value) { this.SET_NAMESPACE(value); },
            },

            currentContext: {
                get() { return this.$store.state.currentContext; },
                set(value) { this.SET_CONTEXT(value); },
            },
            
            ...mapState([
                'isRequestInProgress',
                'contexts',
                'namespaces',
            ]),
            
        },

        mounted() {
            $('.ui.dropdown').dropdown();
        },
        
        methods: {
            ...mapActions([
                REQUEST_NAMESPACES,
            ]),
            ...mapMutations([
                SET_CONTEXT,
                SET_NAMESPACE
            ]),
        },

        watch: {
            currentContext: function(event) {
                
                this.REQUEST_NAMESPACES()
                    .then(result => {
                        
                        console.log(result);
                        // $('#namespace-dropbox').dropdown('set text', "<i class=\"af flag\"></i> 11");
                        // $('#namespace-dropbox').dropdown('set value', '11');
                    })
                    .catch(err => console.log(err));
            }
        }
    }
</script>


<style scoped>
  .ui.container {
    margin-top: 1.5em;
    margin-bottom: 1.5em;
  }
  
  .ui.item {
    margin-right: 1em;
    margin-left: 1em;
  }
</style>
