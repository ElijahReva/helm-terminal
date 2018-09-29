<template lang="html">
   <div class="ui borderless main menu">
    <div class="ui container">
      <div class="right menu">
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
import JQuery from 'jquery'
let $ = JQuery;
export default {
  name: 'MainMenu',
  mounted(){
    // show dropdown on hover
    $('.main.menu  .ui.dropdown').dropdown({
      on: 'hover'
    })
  }
}
</script>

<style scoped>
.item {
    margin-left: 1em;
    margin-right: 1em;
}
.main.menu {
  margin-top: 1em;
  border-radius: 0;
  border: none;
  box-shadow: none;
  transition: box-shadow 0.5s ease, padding 0.5s ease;
}
.ui.container {
  margin-top: 1.5em;
  margin-bottom: 1.5em;
}
</style>
