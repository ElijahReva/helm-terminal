<template lang="html">
  <div class="ui inverted borderless main menu">
    <div class="ui container">
      <div class="right menu">
        <div class="ui item left aligned header">{{ title }}</div>
        <div class="item">Kubernetes Context</div>
        <div class="ui item search selection dropdown" ref="dropdown">
          <input type="hidden" name="country">
          <i class="dropdown icon"></i>
          <div class="default text">Select Context</div>
          <div class="menu">
              <div
                  v-for="context in contexts"
                  v-bind:data-value="context"
                  class="item">
                {{ context }}
              </div>
          </div>
        </div>
        <div class="ui item search aligned selection dropdown" ref="dropdown1">
          <input type="hidden" name="country">
          <i class="dropdown icon"></i>
          <div class="default text">Select Namespace</div>
          <div class="menu">
            <div class="item" data-value="af"><i class="af flag"></i>Afghanistan</div>
            <div class="item" data-value="zm"><i class="zm flag"></i>Zambia</div>   
            <div class="item" data-value="zw"><i class="zw flag"></i>Zimbabwe</div>
          </div>
        </div>
        <div class="item">Kubernetes Namespace</div>
      </div>
    </div>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'

const { mapState, mapActions } = createNamespacedHelpers('signalr');

export default {
    name: 'Header',
    props: [ 'title' ],
    data(){
      return {
          contexts: ['1','2','3']
      }  
    },
    mounted() {
        //  this.$store.dispatch(REQUEST_CONTEXTS);
        let a = $('.ui.dropdown');
        console.log(this);
        console.log(a);        
        // this.$refs.dropdown.dropdown();
        this.$nextTick(function () {
            this.$refs.dropdown.dropdown(); 
            this.$refs.dropdown1.dropdown(); 
        });
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
