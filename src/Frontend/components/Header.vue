<template lang="html">
  <div class="ui inverted borderless main menu">
    <div class="ui container">
      <div class="right menu">
        <div class="ui item left aligned header">{{ title }}</div>
        <div class="item">Kubernetes Context</div>
        <div class="ui  item search selection dropdown">
          <input type="hidden" name="country">
          <i class="dropdown icon"></i>
          <div class="default text">Select Context</div>
          <div class="menu">
            <div class="item" data-value="af"><i class="af flag"></i>Afghanistan</div>
            <div class="item" data-value="zm"><i class="zm flag"></i>Zambia</div>
            <div class="item" data-value="zw"><i class="zw flag"></i>Zimbabwe</div>
          </div>
        </div>
        <div class="ui  item search aligned selection dropdown">
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
    mounted() {
        this.$store.dispatch(REQUEST_CONTEXTS);
        $('.ui.dropdown').dropdown();
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
