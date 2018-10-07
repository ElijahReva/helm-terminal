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
                  :value="ns"
                  :key="ns"
                  class="item">
            
            {{ ns }}

          </option >
        </select>        
        <div class="item">Kubernetes Namespace</div>
      </div>
    </div>
  </div>
</template>
<script>
    import { mapState, mapActions } from 'vuex'
    import { 
        REQUEST_CONTEXTS,
        SET_CONTEXT,
        SET_NAMESPACE,
    } from "../store/rootConstants";
    import JQuery from 'jquery'
    
    let $ = JQuery;    

    export default {
        name: 'Header',
        props: [ 'title' ],
        computed: {
            currentNamespace: {
                get() { return this.$store.state.currentNamespace; },
                set(value) { this.$store.commit(SET_NAMESPACE, value); },
            },

            currentContext: {
                get() { return this.$store.state.currentContext; },
                set(value) { this.$store.commit(SET_CONTEXT, value); },
            },
            
            ...mapState([
                'isRequestInProgress',
                'contexts',
                'namespaces',
            ]),
            // ...mapGetters('manager', [
            //     'yamlButtonColor',
            //     'yamlButtonText',
            //     'hasErrors'
            // ])
        },
    
        created() {
            this.$store.dispatch(REQUEST_CONTEXTS)
        },

        mounted() {
            $('.ui.dropdown').dropdown();
        },

        watch: {
            currentContext: function(event) {
                $('#namespace-dropbox').dropdown('set text', "<i class=\"af flag\"></i> 11");
                $('#namespace-dropbox').dropdown('set value', '11');
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
